using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lab13
{
    public partial class Form1 : Form
    {
        private List<Student> students;

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudents();
            LoadGroups();
            RefreshDataGridView();
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void LoadStudents()
        {
            students = LoadStudentsFromFile("students.txt");
            if (students.Count == 0)
            {
                MessageBox.Show("Студенты не найдены. Проверьте файл students.txt.");
            }
        }

        private void LoadGroups()
        {
            var groups = students.Select(s => s.Group).Distinct().ToList();
            comboBox1.DataSource = groups;
            if (groups.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Группы не найдены.");
            }
        }

        private List<Student> LoadStudentsFromFile(string filePath)
        {
            var students = new List<Student>();
            try
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        students.Add(new Student
                        {
                            LastName = parts[0].Trim(),
                            Group = parts[1].Trim(),
                            Course = int.Parse(parts[2].Trim())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке студентов: {ex.Message}");
            }
            return students;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddStudent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddStudent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddStudent();
        }

        private void AddStudent()
        {
            string group = textBox1.Text.Trim();
            string newStudent = textBox3.Text.Trim();

            if (!string.IsNullOrWhiteSpace(group) && !string.IsNullOrWhiteSpace(newStudent))
            {
                if (students.Any(s => s.Group.Equals(group, StringComparison.OrdinalIgnoreCase)))
                {
                    var newStudentData = new Student
                    {
                        LastName = newStudent,
                        Group = group,
                        Course = 0
                    };
                    students.Add(newStudentData);
                    RefreshDataGridView();
                    textBox3.Clear();
                }
                else
                {
                    MessageBox.Show("Группа не найдена.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void RefreshDataGridView()
        {
            var selectedGroup = comboBox1.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedGroup))
            {
                var filteredStudents = students.Where(s => s.Group.Equals(selectedGroup, StringComparison.OrdinalIgnoreCase))
                                                .Select(s => new
                                                {
                                                    КодСтудента = students.IndexOf(s) + 1,
                                                    Фамилия = s.LastName,
                                                    Группа = s.Group
                                                })
                                                .ToList();
                dataGridView1.DataSource = filteredStudents;
                if (!filteredStudents.Any())
                {
                    MessageBox.Show("Нет студентов в выбранной группе.");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void buttonFetchByCourse_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox3.Text, out int course))
            {
                var studentsByCourse = students.Where(s => s.Course == course).ToList();
                dataGridView1.DataSource = studentsByCourse.Select(s => new {
                    КодСтудента = students.IndexOf(s) + 1,
                    Фамилия = s.LastName,
                    Группа = s.Group
                }).ToList();
                if (!studentsByCourse.Any())
                {
                    MessageBox.Show("Нет студентов на выбранном курсе.");
                }
            }
            else
            {
                MessageBox.Show("Введите корректный номер курса.");
            }
        }
    }

    public class Student
    {
        public string LastName { get; set; }
        public string Group { get; set; }
        public int Course { get; set; }
    }
}
