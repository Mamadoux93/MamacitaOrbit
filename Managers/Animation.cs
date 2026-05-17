using MamacitaOrbit.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamacitaOrbit.Managers
{
    internal static class Animation
    {

        // Made by Chatos Gptos
        public static List<Texture2D> LoadFrames(string folderPath)
        {
            var frames = new List<(int index, Texture2D tex)>();

            try
            {
                foreach (var file in Directory.GetFiles(folderPath))
                {
                    if (!(file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                        file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)))
                        continue;

                    string name = Path.GetFileNameWithoutExtension(file);

                    if (!int.TryParse(name, out int index))
                        continue;

                    using var stream = File.OpenRead(file);

                    frames.Add((index, Texture2D.FromStream(Globals.GraphicsDevice, stream)));
                }
            }
            catch { }

            frames.Sort((a, b) => a.index.CompareTo(b.index));

            return [.. frames.Select(f => f.tex)];
        }
        // Made by Chatos Gptos
    }
}
