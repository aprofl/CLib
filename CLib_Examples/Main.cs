

namespace CLib_Examples
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public void Main_Load(object sender, EventArgs e)
        {
            CLib.Cosys.Init();
        }
    }
}
