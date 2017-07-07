using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Person kiven = new Person() { _name ="kiven",_age =23,_gender ="man"};
            ChangePerson(kiven );
            string name = kiven._name;
            int age = kiven._age;
            string gender = kiven._gender;
        }
        private void ChangePerson(Person person)
        {
            person._name = "change";
            person._age = 2;
            person._gender = "midle";
        }
    }

    public struct Person
    {
        public string _name;//
        public int _age;
        public string _gender;
    }
}
