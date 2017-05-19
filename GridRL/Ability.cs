using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridRL {
    class Ability : ImageSprite {
        /* Properties */
        public string Name { get; set; }

        public string Description { get; set; }

        public int GridWidth { get; set; }

        public int GridHeight { get; set; }

        public int GridY { get; set; }

        public int GridX { get; set; }

        public int CoordY { get; set; }

        public int CoordX { get; set; }

        public virtual void OnUse() { }
    }
}
