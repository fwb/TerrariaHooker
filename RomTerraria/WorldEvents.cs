using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RomTerraria {
    
    static class WorldEvents {

        private static Assembly terrariaAssembly;
        private static Type worldGen;
        private static MethodInfo dropMeteor;
        private static MethodInfo meteor;

        private delegate void SpawnMeteorCallback( );

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
        public static void SpawnMeteor( ) {
            var d = new SpawnMeteorCallback( SpawnMeteorCB );
        }

        private static void SpawnMeteorCB( ) {
            if( meteor != null ) {
                try {
                    var rand = new Random( );
                    var x = rand.Next( 50, Terraria.Main.maxTilesX - 50 );
                    var y = rand.Next( 50, Terraria.Main.maxTilesY - 50 );

                    while( !( Terraria.Main.tile[x, y].active ) || !( Terraria.Main.tileSolid[(int)Terraria.Main.tile[x, y].type] ) ) {
                        x = rand.Next( 50, Terraria.Main.maxTilesX - 50 );
                        y = rand.Next( 50, Terraria.Main.maxTilesY - 50 );
                    }
                    //meteorSpawnX.Text = x.ToString();
                    //meteorSpawnY.Text = y.ToString();

                    meteor.Invoke( null, new object[] { x, y } );
                }
                catch {
                    //
                }
            }
        }

#endregion Meteors
    }

}
