using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Global {
    interface IRectangleCollision {
        //0 : Aucune collision
        //1 : Extérieur vers intérieur bloqué
        //2 : Intérieur vers extérieur bloqué
        //3 : Tout bloqué
        //side[0] : Haut
        //side[1] : Bas
        //side[2] : Gauche
        //side[3] : Droite
        int[] side { get; }
        float x { get; }
        float y { get; }
        float width { get; }
        float height { get; }
    }
}
