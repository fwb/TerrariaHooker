using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace RomTerraria {
    
    public class WorldEvents {

        private static Assembly terrariaAssembly;
        private static Type worldGen;
        private static MethodInfo dropMeteor;
        private static MethodInfo meteor;
        private delegate void SpawnMeteorCallback();

        static WorldEvents( ) {

            terrariaAssembly = Assembly.GetAssembly( typeof( Terraria.Main ) );
            if( terrariaAssembly == null ) {
                return;
            }
            worldGen = terrariaAssembly.GetType( "Terraria.WorldGen" );
            foreach( var f in worldGen.GetMethods( BindingFlags.Static | BindingFlags.Public ) ) {
                if( f.Name == "dropMeteor" ) {
                    dropMeteor = f;
                }
                if( f.Name == "meteor" ) {
                    meteor = f;
                }
            }
        }

#region Meteors - amckhome@tpg.com.au
        public static void SpawnMeteorCB( ) {
            if( meteor != null ) {
                try {
                    var rand = new Random( );
                    var x = rand.Next( 50, Terraria.Main.maxTilesX - 50 );
                    var y = rand.Next( 50, Terraria.Main.maxTilesY - 50 );

                    while( !( Terraria.Main.tile[x, y].active ) || !( Terraria.Main.tileSolid[(int)Terraria.Main.tile[x, y].type] ) ) {
                        x = rand.Next( 50, Terraria.Main.maxTilesX - 50 );
                        y = rand.Next( 50, Terraria.Main.maxTilesY - 50 );
                    }

                    //WhatDelegate del = (WhatDelegate)Delegate.CreateDelegate(typeof(WhatDelegate), meteor);
                    //del.Invoke(x, y);
                    //meteor.Invoke(theInstance, new object[] { x, y });

                    //i think this only works from in Commands.cs because it's being called from a method
                    //run while handling packets, so it's within the Terraria instance.
                    //previously we could run code inside the instance from within makeitharder, as it was
                    //a GameComponent loaded into terraria.
                    meteor.Invoke(null, new object[] { x, y } );


                }
                catch (Exception ext) {
                    Console.Write(ext);
                }
            }
        }

#endregion Meteors
    }

}
