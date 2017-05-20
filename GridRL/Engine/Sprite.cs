using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace GridRL {
    /// <summary> Enumeration of whether a Sprite will call Act on all children, only itself, or not at all.</summary>
    public enum ActingType { AllChildren, ThisOnly, Inactive }
    /// <summary> Enumeration of whether a Sprite will call Render on all children, only itself, or not at all.</summary>
    public enum DrawingType { AllChildren, ThisOnly, Invisible }

    /// <summary> Class for handling basic functions common to all sprites</summary>
    public class Sprite {
        #region Constructors

        public Sprite() { }

        /// <summary> Sets the new sprite's parent to another Sprite object. </summary>
        /// <param name="parent">The sprite that should own the new sprite. </param>
        public Sprite(Sprite parent) {
            parent.AddChild(this);
        }

        #endregion
        #region Properties

        /// <summary> The sprite object whose list of children contains this sprite. </summary>
        public Sprite Parent { get; set; } = null;

        /// <summary> The horizontal location of the sprite, in pixels. </summary>
        public float X { get; set; } = 0;

        /// <summary> The vertical location of the sprite, in pixels. </summary>
        public float Y { get; set; } = 0;

        /// <summary> The scale the sprite should be multiplied by during rendering. Applies to all children. </summary>
        public float Scale { get; set; } = 1;

        /// <summary> The sprite's method of acting. </summary>
        public ActingType ActingType { get; set; } = ActingType.AllChildren;

        /// <summary> The sprite's method of drawing. </summary>
        public DrawingType DrawingType { get; set; } = DrawingType.AllChildren;

        /// <summary> The children who will be rendered/updated when this sprite renders/updates. </summary>
        protected List<Sprite> children = new List<Sprite>();

        #endregion
        #region Methods 

        /// <summary> Adds a Sprite object as a child of this sprite. </summary>
        /// <param name="s"> The sprite to be added. </param>
        protected virtual void AddChild(Sprite s) {
            s.Parent = this;
            children.Add(s);
        }

        /// <summary> Removes a Sprite object from this sprite's list of children. </summary>
        /// <param name="s"> The sprite to be removed. </param>
        protected virtual void RemoveChild(Sprite s) {
            children.Remove(s);
        }

        /// <summary> Adds a sprite to this sprite. </summary>
        /// <param name="s"> The sprite to be added. </param>
        public void Add(Sprite s) {
            AddChild(s);
        }

        /// <summary> Removes a sprite from this sprite's children. </summary>
        /// <param name="s"> The sprite to be removed. </param>
        public void Remove(Sprite s) {
            RemoveChild(s);
        }

        /// <summary> Extensible method designed to handle all action. </summary>
        /// <see cref="Update"/>
        protected virtual void Act() { }

        /// <summary> Extensible method designed to handle all drawing to screen. </summary>
        /// <param name="g"> The transformed graphics</param>
        /// <see cref="Render"/>
        protected virtual void Paint(Graphics g) { }

        /// <summary> Handles updates of this sprite and any children depending upon the ActionType. </summary>
        public void Update() {
            if(ActingType <= ActingType.ThisOnly) {
                Act();
                if(ActingType == ActingType.AllChildren) {
                    foreach(Sprite s in children) {
                        s.Update();
                    }
                }
            }
        }

        /// <summary> Handles rendering of this sprite and any children depending upon the DrawingType. </summary>
        public void Render(Graphics g) {
            if(DrawingType <= DrawingType.ThisOnly) {
                Matrix original = g.Transform.Clone();
                g.TranslateTransform(X, Y);
                g.ScaleTransform(Scale, Scale);
                Paint(g);
                if(DrawingType == DrawingType.AllChildren) {
                    foreach(Sprite s in children) {
                        s.Render(g);
                    }
                }
                g.Transform = original;
            }
        }

        #endregion
    }
}
