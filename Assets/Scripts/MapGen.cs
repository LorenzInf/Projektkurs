using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen {
     public static Room?[,] Gen(int width, int height, int amountOfRooms) {
            var random = new Random();
            var map = new Room?[width, height];
            
            var coords = new Coord(random.Next(0, width), random.Next(0, height));
            map[coords.X, coords.Y] = new Room(RoomType.Empty, new List<Dir>());
            
            FillRec(map, coords, amountOfRooms - 1, random);

            return map;
     }

     private static void FillRec(Room?[,] map, Coord coord, int amountOfRooms, Random random) {
         var paths = FreeAround(map, coord); if (paths.Count < 1 || amountOfRooms < 1) { 
             Console.WriteLine($"amount left: {amountOfRooms}, paths: {paths.Count}");
             return;
         }
            
         var amount = random.Next(paths.Count - 1) + 1; 
         //var amount = paths.Count;
         var nDistribution = Distribute(amountOfRooms, amount, random);
         
         //Console.WriteLine(amount);
         
         for (var i = 0; i < nDistribution.Count; i++) { 
             var pCoord = paths[i];
             
             map[coord.X, coord.Y]?.Dirs.Add(pCoord.Item2); 
             var room = new Room(GetRandomRoom(), new List<Dir> { Opposite(pCoord.Item2) }); 
             map[pCoord.Item1.X, pCoord.Item1.Y] = room;
             
             FillRec(map, pCoord.Item1, nDistribution[i] - 1, random);
         }
     }

     private static RoomType GetRandomRoom() { 
         return RoomType.Boss;
     }

    private static List<int> Distribute(int total, int amount, Random random) { 
        var distribution = new List<double>(); 
        double sum = 0; 
        for (var i = 0; i < amount; i++) { 
            var d = random.NextDouble(); 
            sum += d; 
            distribution.Add(d);
        }
            
        return distribution.Select(d => (int) Math.Round(d / sum * total)).ToList();
    }

    private static List<(Coord, Dir)> FreeAround(Room?[,] rooms, Coord coord) { 
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
    
    private enum Dir { 
        Left, 
        Right, 
        Up, 
        Down
    }
        
    private enum RoomType {
        Empty, 
        Enemy, 
        Boss, 
        Loot
    }
    
    private static List<T> GetEnumList<T>() { 
        var enumList = Enum.GetValues(typeof(T))
            .Cast<T>().ToList();
        return enumList;
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

    private readonly struct Coord { 
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

    private readonly struct Room { 
        public readonly RoomType Type; 
        public readonly List<Dir> Dirs;
        
        public Room(RoomType type, List<Dir> dirs) { 
            Type = type; 
            Dirs = dirs;
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
