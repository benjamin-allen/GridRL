using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace GridRL
{
    public enum ActingType { AllChildren, ThisOnly, Inactive }
    public enum DrawingType { AllChildren, ThisOnly, Invisible}
    public class Sprite
    {
        /* Constructors */
        public Sprite() { }

        public Sprite(Sprite parent) {
            parent.AddChild(this);
        }


        /* Properties */
        protected List<Sprite> children = new List<Sprite>();
        protected Sprite parent = null;
        protected double x = 0;
        protected double y = 0;
        protected double scale = 1;
        private ActingType actingType = ActingType.AllChildren;
        private DrawingType drawingType = DrawingType.AllChildren;

        public double X {
            get { return x; }
            set { x = value; }
        }
        public double Y {
            get { return y; }
            set { y = value; }
        }
        public double Scale {
            get { return scale; }
            set { scale = value; }
        }
        public Sprite Parent {
            get { return parent; }
            set { parent = value; }
        }
        public DrawingType DrawingType {
            get { return drawingType; }
            set { drawingType = value; }
        }
        public ActingType ActingType {
            get { return actingType; }
            set { actingType = value; }
        }


        /* Children/Parent methods */
        protected virtual void AddChild(Sprite s) {
            s.parent = this;
            children.Add(s);
        }
        protected virtual void RemoveChild(Sprite s) {
            s.parent = null;
            children.Remove(s);
        }


        /* Update/Render methods */
        protected virtual void Act() { }
        protected virtual void Paint(Graphics g) { g.DrawRectangle(Pens.Aqua, (float)0, (float)0, 100, 100); }

        public void Update() {
            if(actingType <= ActingType.ThisOnly) {
                Act();
                if(actingType == ActingType.AllChildren) {
                    foreach(Sprite s in children) {
                        s.Update();
                    }
                }
            }
        }

        public void Render(Graphics g) {
            if(drawingType <= DrawingType.ThisOnly) {
                Matrix original = g.Transform.Clone();
                g.TranslateTransform((float)x, (float)y);
                g.ScaleTransform((float)scale, (float)scale);
                Paint(g);
                foreach(Sprite s in children) {
                    s.Render(g);
                }
                g.Transform = original;
            }
        }
    }
}
