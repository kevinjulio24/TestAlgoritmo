using ConsoleApp3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.IO.File;

namespace ConsoleApp3
{
    internal class Program
    {
        private enum Direction { Norte, Sur, este, Oeste }

        private static List<Nodos> _nodos = new List<Nodos>();

        private static int[][] _puntos;
        private static int IdValorEjecucion { get; set; } = 1;
        private static int NivelCarrera  { get; set; } = 0;

        
        static void Main(string[] args)
        {
            Console.WriteLine("Trabajando......");

            _puntos = ReadLines(@"C:\Users\KEVIN ANDRES JULIO\Downloads\skirsesort.kitzbuehel (1)\map.txt")
                     .Skip(1).Select(x => x.Split(' ').Select(int.Parse).ToArray()).ToArray();

            for (var i = 0; i < _puntos.Length; i++)
            {
                for (var x = 0; x < _puntos[i].Length; x++)
                {
                    _nodos.Add(new Nodos(IdValorEjecucion++, new Puntos(i, x), _puntos[i][x], 0, 0));
                }
            }

            while (AgregarSubnodo() > 0) { }

            var rutaMásLarga = _nodos.OrderByDescending(x => x.Level).FirstOrDefault()!.Level;

            var nodosDeRuta = new List<int[]>();

            foreach (var y in _nodos.Where(y => y.Level == rutaMásLarga)
                         .OrderByDescending(x => x.Id).ToList()) nodosDeRuta.Add(ObtenerValoreNodo(y));

            var masempinado = nodosDeRuta.OrderByDescending(x => x.Max() - x.Min()).FirstOrDefault();
            Console.WriteLine("Longitud de la ruta calculada: {0}", rutaMásLarga + 1);
            Console.WriteLine("Caída de la ruta calculada: {0}", masempinado!.Max() - masempinado.Min() );
            var list = masempinado.ToList();
            Console.WriteLine("Ruta calculada:");
            foreach (var x in list) Console.WriteLine(x);
            Console.ReadLine();
           
        }
        static int[] ObtenerValoreNodo(Nodos node)
        {
            var rutaNodo = new List<int>() { node.NodeV };
            Nodos nodoPadre = null;
            do
            {
                nodoPadre = _nodos.SingleOrDefault(x => x.Id == node.ParentId);
                if (nodoPadre != null)
                    rutaNodo.Add(nodoPadre.NodeV);
                node = nodoPadre;
            } while (nodoPadre != null);
            return rutaNodo.ToArray();
        }


        private static int AgregarSubnodo()
        {
            int contar = 0;
            foreach (var x in _nodos.Where(x => x.Level == NivelCarrera).ToArray().ToList())
            {
                contar += agregarpuntos(Direction.Norte, x) + agregarpuntos(Direction.Sur, x) + agregarpuntos(Direction.este, x) + agregarpuntos(Direction.Oeste, x);
            }
            NivelCarrera++;
            return contar;
        }

        static int agregarpuntos(Direction dir, Nodos node)
        {
            Puntos n = Encontrarpuntos(dir, node.Loc);
            if (n != null && _puntos[n.X][n.Y] < node.NodeV)
            {
                _nodos.Add(new Nodos(IdValorEjecucion++, n, _puntos[n.X][n.Y], node.Id, NivelCarrera + 1));
                return 1;
            }
            return 0;
        }
        static Puntos Encontrarpuntos(Direction d, Puntos n)
        {
            Puntos siguientePunto;
            if (d != Direction.Norte)
            {
                if (d == Direction.Sur)
                    siguientePunto = n.X + 1 < _puntos.GetLength(0) ? new Puntos(n.X + 1, n.Y) : null;
                else if (d == Direction.este)
                    siguientePunto = n.Y + 1 < _puntos.GetLength(0) ? new Puntos(n.X, n.Y + 1) : null;
                else if (d == Direction.Oeste)
                    siguientePunto = n.Y - 1 >= 0 ? new Puntos(n.X, n.Y - 1) : null;
                else
                    throw new ArgumentOutOfRangeException(nameof(d), d, null);
            }
            else
                siguientePunto = n.X - 1 >= 0 ? new Puntos(n.X - 1, n.Y) : null;

            return siguientePunto;
        }
    }
}
