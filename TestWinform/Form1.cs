using TDMCProtocol;

namespace TestWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                QCPU plc = new QCPU("192.168.0.1", 5000);
                plc.Open();
                if (plc.IsConnected)
                {
                    bool bl = (bool)plc.Read("M1915", VarType.Bit);

                    short aint = (short)plc.Read("D7460", VarType.Int);

                    string idcode = plc.Read("D8370", VarType.String, 9).ToString();


                    plc.Write("M1916", VarType.Bit, false);
                    plc.Write("D7460", VarType.Int, (short)0);

                    plc.Write("D8370", VarType.String, "12345666");
                }
                plc.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(  ex.Message);
            }
        }
    }
}
