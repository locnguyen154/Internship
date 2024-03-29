﻿using System;
using System.Linq;
using System.Windows.Forms;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3
{
    public partial class AddTaskForm : Form
    {
        private BusinessLogic _bll;
        private Task _taskEdit;
        private Author _authorEdit;
        public delegate void PassData(Task task, Author author);
        public delegate void ShowMainForm();
        public PassData passData;
        public ShowMainForm showMainForm;

        public AddTaskForm()
        {
            InitializeComponent();
            btnUpdate.Visible = false;
            _bll = new BusinessLogic();
            LoadAuthor("");
        }

        public AddTaskForm(Task task, Author author)
        {
            InitializeComponent();
            tbTitle.Text = task.Title;
            tbDescription.Text = task.Description;
            btnOk.Visible = false;
            _taskEdit = task;
            _authorEdit = author;
            _bll = new BusinessLogic();            
            LoadAuthor(author.AuthorName);
            cbAuthor.SelectedItem = author.AuthorName;
            cbAuthor.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadAuthor(string authorName)
        {
            var authors = _bll.GetAllAuthors();
            foreach (var author in authors)
            {
                cbAuthor.Items.Add(author.AuthorName);
                if (author.AuthorName == authorName)
                {
                    cbAuthor.SelectedItem = authorName;
                }
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var task = new Task
            {
                Id = -1,
                Title = tbTitle.Text,
                Description = tbDescription.Text,
                TimeCreate = DateTime.Now,
                TypeList = (int)TypeList.Todo
            };

            var authors = _bll.GetAllAuthors();
            var author = authors.FirstOrDefault(s => s.AuthorName == cbAuthor.Text);

            if (author == null)
            {
                author = new Author();
                author.AuthorName = cbAuthor.Text;
            }

            if (!_bll.IsEmptyTask(task) && !_bll.IsEmptyAuthor(author))
            {
                passData(task, author);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Please fill in full info", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormAddTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            showMainForm();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            _taskEdit.Title = tbTitle.Text;
            _taskEdit.Description = tbDescription.Text;
            var authors = _bll.GetAllAuthors();
            var author = authors.FirstOrDefault(s => s.AuthorName == cbAuthor.Text);

            if (!_bll.IsEmptyTask(_taskEdit))
            {
                passData(_taskEdit, author);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Please fill in full info", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
