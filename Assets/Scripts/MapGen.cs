using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class MapGen{

	 private static Coord last;
     private static int maxD;
    

     public static Room[,] Gen(int width, int height, int amountOfRooms) {
            maxD = 0;

            var random = new System.Random();
            var map = new Room[width, height];
            
            var coords = new Coord(random.Next(0, width), random.Next(0, height));
            map[coords.X, coords.Y] = new Room(RoomType.Starting, new List<Dir>());
		
            FillRec(map, coords, amountOfRooms - 1, random, 0);

			map[last.X, last.Y] = new Room(RoomType.Boss, map[last.X, last.Y].Dirs);

            return map;
     }

     public delegate void Action();
     
     private static void FillRec(Room[,] map, Coord coord, int amountOfRooms, System.Random random, int depth) {
         var paths = FreeAround(map, coord); 
		 if (paths.Count < 1 || amountOfRooms < 1) {
             return;
         }
            
         //var amount = random.Next(paths.Count - 1) + 1; 
         var amount = paths.Count;
         var nDistribution = Distribute(amountOfRooms, amount, random);
         
         
         for (var i = 0; i < nDistribution.Count; i++) { 
             var pCoord = paths[i];
             
             map[coord.X, coord.Y].Dirs.Add(pCoord.Item2); 
             var room = new Room(GetRandomRoom(random), new List<Dir> { Opposite(pCoord.Item2) }); 
             map[pCoord.Item1.X, pCoord.Item1.Y] = room;
             if (depth > maxD) {
                 last = pCoord.Item1;
                 maxD = depth;
             }
             FillRec(map, pCoord.Item1, nDistribution[i] - 1, random, depth + 1);
         }
     }

     private static RoomType GetRandomRoom(System.Random r) { 
    	 var rooms = new List<RoomType>{
        	RoomType.Enemy,
            RoomType.Enemy,
            RoomType.Enemy,
        	RoomType.Loot,
        	RoomType.Empty
         };
    	 int index = r.Next(rooms.Count);
    	 return rooms[index];
     }

    private static List<int> Distribute(int total, int amount, System.Random random) { 
        var distribution = new List<double>(); 
        double sum = 0; 
        for (var i = 0; i < amount; i++) { 
            var d = random.NextDouble(); 
            sum += d; 
            distribution.Add(d);
        }
            
        return distribution.Select(d => (int) Math.Round(d / sum * total)).ToList();
    }

    private static List<(Coord, Dir)> FreeAround(Room[,] rooms, Coord coord) { 
        var x = coord.X; 
        var y = coord.Y;
        
        var coords = new List<(Coord, Dir)> { 
            (new Coord(x - 1, y), Dir.Left), 
            (new Coord(x + 1, y), Dir.Right), 
            (new Coord(x, y - 1), Dir.Up), 
            (new Coord(x, y + 1), Dir.Down),
        };

        return coords.FindAll(c => InBounds(rooms, c.Item1) &&
                                   rooms[c.Item1.X, c.Item1.Y] == null);
    }
    
    private static bool InBounds<T>(T[,] arr, Coord coord) {
        return 0 <= coord.X && coord.X < arr.GetLength(0) && 
               0 <= coord.Y && coord.Y < arr.GetLength(1);
    }
    
    public enum Dir { 
        Left, 
        Right, 
        Up, 
        Down,
		Null
    }
        
    public enum RoomType {
		Starting,
        Empty, 
        Enemy, 
        Boss, 
        Loot
    }

    private static Dir Opposite(Dir d) { 
        return d switch { 
            Dir.Up => Dir.Down, 
            Dir.Down => Dir.Up, 
            Dir.Left => Dir.Right, 
            Dir.Right => Dir.Left, 
            _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
        };
    }

    public readonly struct Coord { 
        public readonly int X; 
        public readonly int Y;
        
        public Coord(int x, int y) { 
            X = x; 
            Y = y;
        }

        public override string ToString() { 
            return $"({X}, {Y})";
        }
    }

    public class Room { 
        public readonly RoomType Type; 
        public readonly List<Dir> Dirs;

		private Config.Item item=Config.Item.Null;
		private Config.Weapon weapon=Config.Weapon.Null;
		private bool visited;
        
        public Room(RoomType type, List<Dir> dirs) { 
            Type = type; 
            Dirs = dirs;
			visited=false;
			if(type==RoomType.Loot)
				GenerateLoot();
        }

        public string GetLoot(){
            if(item!=Config.Item.Null) return "item";
			if(weapon!=Config.Weapon.Null) return "weapon";
			return "null";
        }

        public Config.Item TakeItem(){
            var itemVar = item;
            item = Config.Item.Null;
            return itemVar;
        }

		public Config.Weapon TakeWeapon(){
            var weaponVar = weapon;
            weapon = Config.Weapon.Null;
            return weaponVar;
        }

        public bool Visited(bool b){
			if(b)
				visited=true;
			return visited;
		}

		private void GenerateLoot(){
			System.Random random=new System.Random();
        	double r=random.Next(0,10);
			if(r<5){
				item=Config.GetRandomItem();
			}else if(r>5){
				weapon=Config.GetRandomWeapon();
			}
		}
        
        public override string ToString() { 
            var l = Dirs.Contains(Dir.Left) ? "<" : " ";
            var r = Dirs.Contains(Dir.Right) ? ">" : " "; 
            var u = Dirs.Contains(Dir.Up) ? "^" : " "; 
            var d = Dirs.Contains(Dir.Down) ? "v" : " ";
            return $"{l}{u} {Type} {r}{d}";
        }
    }
}