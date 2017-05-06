﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace GridRL {
    public enum ActingType { AllChildren, ThisOnly, Inactive }
    public enum DrawingType { AllChildren, ThisOnly, Invisible }
    public class Sprite {
        /* Constructors */
        public Sprite() { }

        public Sprite(Sprite parent) {
            parent.AddChild(this);
        }


        /* Properties */
        public Sprite Parent { get; set; } = null;
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public float Scale { get; set; } = 1;
        public ActingType ActingType { get; set; } = ActingType.AllChildren;
        public DrawingType DrawingType { get; set; } = DrawingType.AllChildren;


        /* Children/Parent methods */
        protected List<Sprite> children = new List<Sprite>();
        protected virtual void AddChild(Sprite s) {
            s.Parent = this;
            children.Add(s);
        }
        protected virtual void RemoveChild(Sprite s) {
            s.Parent = null;
            children.Remove(s);
        }


        /* Update/Render methods */
        protected virtual void Act() { }
        protected virtual void Paint(Graphics g) { }

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
    }
}
