using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vecs = VectSharp;
using VectSharp.Raster;


namespace MonogameUtils
{
    public class TextBox
    {
        private Texture2D _textTexture;
        private string _text;
        public string Text{
            get => _text;
            set{
                _text = value;
                _textLines = _text.Split("\n");
                box.Size = new Point((int)font.MeasureText(_text).Width + 5, ((int)font.MeasureText(_text).Height + 5) * _textLines.Length);
            }
        }
        private string[] _textLines;

        public double fontSize = 20; 
        public Rectangle box;
        public vecs::Colour textFillColor = vecs::Colours.Black;
        public vecs::Colour textStrokeColor = vecs::Colours.Black;
        public vecs::Font font;

        public TextBox(Point position, vecs::FontFamily fontFamily, double _fontSize = 60, string _text = "Lorem Ipsum"){
            fontSize = _fontSize;
            font = new(fontFamily, fontSize);
            Text = _text;
            box = new(position, new Point((int)font.MeasureText(Text).Width + 5, (int)font.MeasureText(Text).Height + 5));
        }

        public Texture2D GetTexture2D(GraphicsDevice gd){
            
            vecs::Page page = new(box.Width, box.Height);
            vecs::Graphics graphics = page.Graphics;

            for (int lineNum = 0; lineNum < _textLines.Length; lineNum++)
            {
                graphics.StrokeText(new vecs::Point(0, lineNum * ((int)font.MeasureText(Text).Height + 5)), _textLines[lineNum], font, textStrokeColor);
                graphics.FillText(new vecs::Point(0, lineNum * ((int)font.MeasureText(Text).Height + 5)), _textLines[lineNum], font, textFillColor);
            }
            Stream stream = new MemoryStream();
            page.SaveAsPNG(stream);
            _textTexture = Texture2D.FromStream(gd, stream);
            return _textTexture;
        }

    }
}