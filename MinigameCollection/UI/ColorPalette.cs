

using Dalamud.Bindings.ImGui;
using System.Numerics;

namespace MinigameCollection.UI
{
    public class ColorPalette
    {
        public Vector4 LightGreen = new Vector4(0, 1, 0, 1);
        public Vector4 MidGreen = new Vector4(0, 0.5f, 0, 1);
        public Vector4 DarkGreen = new Vector4(0, 0.2f, 0, 1);
        public Vector4 LightRed = new Vector4(1, 0, 0, 1);
        public Vector4 MidRed = new Vector4(0.5f, 0, 0, 1);
        public Vector4 DarkRed = new Vector4(0.2f, 0, 0, 1);
        public Vector4 LightBlue = new Vector4(0,0,1, 1);
        public Vector4 MidBlue = new Vector4(0, 0, 0.5f, 1);
        public Vector4 DarkBlue = new Vector4(0, 0, 0.2f, 1);
        public Vector4 White = new Vector4(1, 1, 1, 1);
        public  uint GetRowColor(int row)
        {
            return ImGui.GetColorU32(new Vector4(0.3f, 0.3f, 0.3f, row % 2 != 0 ? 0.65f : 0.45f));
        }
    }
}
