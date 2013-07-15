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
using System.Data.Entity;

namespace AcmeEmployees


{
   
    public partial class FormEmployees : Form
    {

        delegate void MyDelegate(string[] array, bool action);
        private DataAccess.AcmeContext context = new DataAccess.AcmeContext();
        private Employee emplEdited = new Employee();

        public Employee EmplEdited
        {
            get { return emplEdited; }
            set { emplEdited = value; }
        }

        public FormEmployees()
        {
            InitializeComponent();
            CreateListView();

         }

        private void CreateListView() { 

        //Set properties listView
        
            listView1.View = View.Details;
            //listView1.Bounds = new Rectangle(new Point(60, 10), new Size(200, 200));
            // Set the view to show details.
            listView1.View = View.Details;
            // Allow the user to edit item text.
            listView1.LabelEdit = true;
            // Allow the user to rearrange columns.
            listView1.AllowColumnReorder = true;
            // Display check boxes.
            listView1.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            listView1.FullRowSelect = true;
            // Display grid lines.
            listView1.GridLines = true;
            // Sort the items in the list in ascending order.
            listView1.Sorting = SortOrder.Ascending;

            //Values to be populated
            var query = (from empl in context.Employees
                         select new
                         {

                             Name = empl.Name,
                             ID = empl.Id,
                             Address = empl.Address,
                             Telephone = empl.Telephone

                         }).ToList();

            foreach (var itemQuery in query)
            {

                ListViewItem lstItem = new ListViewItem();
           
                lstItem.SubItems.Add(itemQuery.Name);
                lstItem.SubItems.Add(itemQuery.ID.ToString());
                lstItem.SubItems.Add(itemQuery.Address);
                lstItem.SubItems.Add(itemQuery.Telephone);
                listView1.Items.Add(lstItem);

            }

            listView1.Columns.Add("Employee", -2, HorizontalAlignment.Left);
            var columnHeaders = typeof(Employee).GetProperties().Select(m => m.Name).ToList();
            foreach (var item in columnHeaders)
            {
                listView1.Columns.Add(item, -2);

            }

       
        }

        private void deleteEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem  item in listView1.CheckedItems){

                
                    int idToRemove = int.Parse(item.SubItems[2].Text);

                    var employeeToRemove = (from emp in context.Employees
                                            where emp.Id == idToRemove
                                            select emp).SingleOrDefault();

                    if (employeeToRemove != null ) {

                        context.Employees.Remove(employeeToRemove);
                        context.SaveChanges();
                    
                    }
                    
 
                }
        
 
            
        }

        private void newEmployeeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormAddEmployee form = new FormAddEmployee(this);
            form.Show(this);
            form.Owner = this;
        }

        private void removeEmployeeToolStripMenuItem_Click(object sender, EventArgs e)

        {
            if (listView1.CheckedItems.Count == 0)
            {

                MessageBox.Show(this, "You must mark the checkbox/es for remove the employee", "Delete Employee/s", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            foreach (ListViewItem item in listView1.CheckedItems) {

                for (int i = 0; i < item.SubItems.Count; i++) {

                    int idToRemove = int.Parse(item.SubItems[2].Text);

                    var query = (from emp in context.Employees
                                 where emp.Id == idToRemove
                                 select emp).SingleOrDefault();

                    if(query!= null){
  
                        context.Employees.Remove(query);
                        break;
                     
                    }
 
                }
            
            }
            context.SaveChanges();
            if (listView1.CheckedItems.Count > 0) {

          
                var ind = listView1.CheckedIndices;

                for (int i = ind.Count - 1; i >= 0; i--)
                {

                    listView1.Items.RemoveAt(ind[i]);
                
                }
                MessageBox.Show("Employee/s removed", "Delete Employee/s", MessageBoxButtons.OK);
            }
            
                           
        }

        public void UpdatingListView(string[] array, bool action)
        {
            if (this.listView1.InvokeRequired)
                this.listView1.Invoke(new MyDelegate(UpdatingListView), new object[] { array });
            else
            {


                ListViewItem lvi = new ListViewItem();
                for (int i = 0; i < array.Count(); i++)
                {
                    lvi.SubItems.Add(array[i]);

                }



                if (action)
                {
                    
                    
                    this.listView1.Items.Add(lvi);

                }
                else {

                  
                   var checkIn = listView1.CheckedIndices;

                   if (checkIn.Count > 0) {

                       for (int i = 1; i < lvi.SubItems.Count; i++)
                       {


                           if (!String.IsNullOrEmpty(array[i-1]))
                               listView1.Items[checkIn[0]].SubItems[i].Text = array[i - 1];

                       }
                   }

                  
                   

                }
               
            }
        }

        private void editEmployeeToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (listView1.CheckedItems.Count != 1) { MessageBox.Show(this, "You should check (one) item for be edit", "Edit Employee", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {

                var items = listView1.CheckedItems[0].SubItems;

                for (int i = 1; i < items.Count; i++) {

                    emplEdited.Name = items[i].Text;
                    emplEdited.Id = int.Parse(items[i+i].Text);
                    emplEdited.Address = items[i + i + i].Text ;
                    emplEdited.Telephone = items[i + i + i + i].Text;
                    break;
                
                }
                            

                FormAddEmployee form = new FormAddEmployee(this, emplEdited);
                form.Show(this);
                form.Owner = this;
            }
           
        }
       
    }

    //public static class ListViewExtensions
    //{
    //    public static ListView AddItems(this ListView listView,
    //        IEnumerable<ListViewItem> items)
    //    {
    //        listView.Items.AddRange(items.ToArray());
    //        return listView;
    //    }

    //    public static ListViewItem WithSubItems(this ListViewItem item,
    //        IEnumerable<string> subItems)
    //    {
    //        item.SubItems.AddRange(subItems.ToArray());
    //        return item;
    //    }
    //}
}
