using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

//Main form
namespace AcmeEmployees
{   
    public partial class FormAddEmployee : Form
    {
        private FormEmployees _mainForm;
        private bool _edit;

        public bool Edit
        {
            get { return _edit; }
            set { _edit = value; }
        }
        public FormEmployees MainForm
        {
            get { return _mainForm; }
            set { _mainForm = value; }
        }
        private DataAccess.AcmeContext context = new DataAccess.AcmeContext();
        private ErrorProvider errorProviderId = new ErrorProvider();
        
        public FormAddEmployee(FormEmployees main, Employee emp)
        {
            InitializeComponent();
            _mainForm = main;
            Edit = true;
            textBoxID.Enabled = false; 
            FillValuesToEdit(emp);
         

           
           
        }
        public FormAddEmployee(FormEmployees main) {

            InitializeComponent();
            _mainForm = main;
           
        }

        private void FillValuesToEdit(Employee emp) {

            textBoxName.Text = emp.Name;
            textBoxID.Text = emp.Id.ToString();
            textBoxAddress.Text = emp.Address;
            textBoxTelephone.Text = emp.Telephone;
        
        
        }

        private bool SaveValues()
        {

            if (!Edit) {

                int id;
                if (int.TryParse(textBoxID.Text, out id))
                {

                    var emp = new Employee
                    {

                        Name = textBoxName.Text,
                        Id = int.Parse(textBoxID.Text),
                        Address = textBoxAddress.Text,
                        Telephone = textBoxTelephone.Text

                    };

                    context.Employees.Add(emp);
                    context.SaveChanges();
                    MessageBox.Show("Employee: " + "ID: " + textBoxID.Text + " has been saved.", "Save", MessageBoxButtons.OK);
                   
                }
                else {

                    MessageBox.Show(this,"Employee: " + "ID: " + textBoxID.Text + " error (NaN)", "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorProviderId.SetError(textBoxID, "ID is not a nummber");
                    return false;
                }
            }
            return true;
        }

        private void UpdateListView()
        {

            string Name = textBoxName.Text;
            string Id = textBoxID.Text;
            string Address = textBoxAddress.Text;
            string Telephone = textBoxTelephone.Text;
 
            string[] array = new string[4] { Name, Id, Address, Telephone };

            if (!Edit)
            {
                _mainForm.UpdatingListView(array, true);
            }
            else {

                _mainForm.UpdatingListView(array, false);
       
            
            }
            
        }


        private Boolean ValidateValues() {

            Boolean valid = true;

            errorProviderName.Clear();
            errorProviderId.Clear();

            if (!String.IsNullOrEmpty(textBoxName.Text))
            {
                string name = textBoxName.Text;

                if (!Edit) {

                    var query = (from emp in context.Employees
                                 where emp.Name.Contains(name)
                                 select emp).Count();

                    if (query > 0)
                    {
                        errorProviderName.SetError(textBoxName, "Employee already exists");
                        valid = false;
                    }
         
                
                
                }
                   
             
            }
            else {

                errorProviderName.SetError(textBoxName, "Please, provide a correct Firstname with Surname");
                valid = false;
     
            }

      
                if (!String.IsNullOrEmpty(textBoxID.Text))
                {

                    if (!Edit)
                    {

                        try
                        {
                            int checkId = int.Parse(textBoxID.Text);
                            var query = (from emp in context.Employees
                                         where emp.Id == checkId
                                         select emp).SingleOrDefault();

                            if (query != null)
                            {

                                errorProviderId.SetError(textBoxID, "ID is used, please provide different one");
                                valid = false;
                            }

                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("ID should be an integer");
                            
                        }

                        
                        
                    }
                   
            
                }

                else {
                    errorProviderId.SetError(textBoxID, "ID is empty");
                    valid = false;
                }
    
           return valid;

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (ValidateValues()) {

                errorProviderName.Clear();
                errorProviderId.Clear();
                if (SaveValues())
                    UpdateListView();

                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

      
    }
}
