namespace SahApp.Server
{
    public class Player
    {
        private string Name { get; set; }
        private bool Color { get; set; }

        public Player() {

            this.Name = "";

        }

        public Player(string name)
        {
            this.Name = name;
      
        }

     
        public Player(string name, bool color) : this(name)
        {
            this.Color = color;
        }


        public string GetName()
        {
            return Name;
        }

        public bool GetColor()
        {
            return Color;
        }

    }
}
