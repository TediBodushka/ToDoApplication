using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer;
using DataLayer;

namespace ToDoConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var dbContext = new ToDoListDbContext();

            if (!dbContext.Users.Any())
            {
                var defaultUser = new User(
                    username: "defaultUser",
                    email: "default@example.com",
                    passwordHash: "hashed-password-here" 
                );

                dbContext.Users.Add(defaultUser);
                dbContext.SaveChanges();
            }

            int defaultUserId = dbContext.Users.First().Id;

            var categoryContext = new CategoryContext(dbContext);
            var taskContext = new TaskItemContext(dbContext);

            MainMenu(categoryContext, taskContext, defaultUserId);
        }

        private static void MainMenu(CategoryContext categoryContext, TaskItemContext taskContext, int defaultUserId)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== MAIN MENU =====");
                Console.WriteLine("1. Manage Category");
                Console.WriteLine("2. Manage Task");
                Console.WriteLine("0. Exit");
                Console.Write("Choose option: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CategoryMenu(categoryContext);
                        break;
                    case "2":
                        TaskMenu(taskContext, categoryContext, defaultUserId);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        #region Category Menu

        private static void CategoryMenu(CategoryContext categoryContext)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== CATEGORY MENU =====");
                Console.WriteLine("1. Create Category");
                Console.WriteLine("2. Delete Category");
                Console.WriteLine("3. Read All Categories");
                Console.WriteLine("4. Update Category");
                Console.WriteLine("0. Back");
                Console.Write("Choose option: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateCategory(categoryContext);
                        break;
                    case "2":
                        DeleteCategory(categoryContext);
                        break;
                    case "3":
                        ReadAllCategories(categoryContext);
                        break;
                    case "4":
                        UpdateCategory(categoryContext);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void CreateCategory(CategoryContext categoryContext)
        {
            Console.Clear();
            Console.WriteLine("===== CREATE CATEGORY =====");
            Console.Write("Title: ");
            string title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title is required. Press any key to return...");
                Console.ReadKey();
                return;
            }

            var category = new Category(title);
            categoryContext.Create(category);

            Console.WriteLine("Category created successfully!");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void DeleteCategory(CategoryContext categoryContext)
        {
            Console.Clear();
            Console.WriteLine("===== DELETE CATEGORY =====");

            var categories = categoryContext.ReadAll();
            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            PrintCategories(categories);

            Console.Write("Choose category number to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int number) ||
                number < 1 || number > categories.Count)
            {
                Console.WriteLine("Invalid number. Press any key to return...");
                Console.ReadKey();
                return;
            }

            var categoryToDelete = categories[number - 1];

            try
            {
                categoryContext.Delete(categoryToDelete.Id);
                Console.WriteLine("Category deleted successfully!");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Category not found.");
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void ReadAllCategories(CategoryContext categoryContext)
        {
            Console.Clear();
            Console.WriteLine("===== ALL CATEGORIES =====");

            var categories = categoryContext.ReadAll();

            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
            }
            else
            {
                PrintCategories(categories);
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void UpdateCategory(CategoryContext categoryContext)
        {
            Console.Clear();
            Console.WriteLine("===== UPDATE CATEGORY =====");

            var categories = categoryContext.ReadAll();
            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            PrintCategories(categories);

            Console.Write("Choose category number to update: ");
            if (!int.TryParse(Console.ReadLine(), out int number) ||
                number < 1 || number > categories.Count)
            {
                Console.WriteLine("Invalid number. Press any key to return...");
                Console.ReadKey();
                return;
            }

            var category = categories[number - 1];

            Console.Write($"New Title (current: {category.Title}): ");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                category.Title = newTitle;
                categoryContext.Update(category);
                Console.WriteLine("Category updated successfully!");
            }
            else
            {
                Console.WriteLine("Title unchanged.");
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void PrintCategories(List<Category> categories)
        {
            Console.WriteLine("No.\tTitle");
            Console.WriteLine("-------------------------");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}\t{categories[i].Title}");
            }
        }

        #endregion

        #region Task Menu

        private static void TaskMenu(TaskItemContext taskContext, CategoryContext categoryContext, int defaultUserId)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== TASK MENU =====");
                Console.WriteLine("1. Create Task");
                Console.WriteLine("2. Delete Task");
                Console.WriteLine("3. Read All Tasks");
                Console.WriteLine("4. Update Task");
                Console.WriteLine("0. Back");
                Console.Write("Choose option: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateTask(taskContext, categoryContext, defaultUserId);
                        break;
                    case "2":
                        DeleteTask(taskContext);
                        break;
                    case "3":
                        ReadAllTasks(taskContext);
                        break;
                    case "4":
                        UpdateTask(taskContext, categoryContext);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void CreateTask(TaskItemContext taskContext, CategoryContext categoryContext, int defaultUserId)
        {
            Console.Clear();
            Console.WriteLine("===== CREATE TASK =====");

            Console.Write("Title: ");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title is required. Press any key to return...");
                Console.ReadKey();
                return;
            }

            Console.Write("Description (optional): ");
            string description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                description = null;
            }

            DateTime dueDate;
            while (true)
            {
                Console.Write("Due date (yyyy-MM-dd): ");
                string dueDateStr = Console.ReadLine();
                if (DateTime.TryParse(dueDateStr, out dueDate))
                {
                    break;
                }
                Console.WriteLine("Invalid date format. Please try again.");
            }

            // CATEGORY SELECTION BY NUMBER (1..N)
            var categories = categoryContext.ReadAll();
            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found. Please create a category first.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Available Categories:");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i].Title}");
            }

            int chosenIndex;
            while (true)
            {
                Console.Write("Choose category by number: ");
                if (int.TryParse(Console.ReadLine(), out chosenIndex) &&
                    chosenIndex >= 1 && chosenIndex <= categories.Count)
                {
                    break;
                }
                Console.WriteLine("Invalid choice. Please try again.");
            }

            var selectedCategory = categories[chosenIndex - 1];

            int userId = defaultUserId;

            var task = new TaskItem(title, description, dueDate, false, selectedCategory.Id, userId);
            taskContext.Create(task);

            Console.WriteLine("Task created successfully!");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void DeleteTask(TaskItemContext taskContext)
        {
            Console.Clear();
            Console.WriteLine("===== DELETE TASK =====");

            var tasks = taskContext.ReadAll(useNavigationalProperties: true);
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            PrintTasks(tasks);

            Console.Write("Choose task number to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int number) ||
                number < 1 || number > tasks.Count)
            {
                Console.WriteLine("Invalid number. Press any key to return...");
                Console.ReadKey();
                return;
            }

            var taskToDelete = tasks[number - 1];

            try
            {
                taskContext.Delete(taskToDelete.Id);
                Console.WriteLine("Task deleted successfully!");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Task not found.");
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void ReadAllTasks(TaskItemContext taskContext)
        {
            Console.Clear();
            Console.WriteLine("===== ALL TASKS =====");

            var tasks = taskContext.ReadAll(useNavigationalProperties: true);

            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
            }
            else
            {
                PrintTasks(tasks);
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void UpdateTask(TaskItemContext taskContext, CategoryContext categoryContext)
        {
            Console.Clear();
            Console.WriteLine("===== UPDATE TASK =====");

            var tasks = taskContext.ReadAll(useNavigationalProperties: true);
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            PrintTasks(tasks);

            Console.Write("Choose task number to update: ");
            if (!int.TryParse(Console.ReadLine(), out int number) ||
                number < 1 || number > tasks.Count)
            {
                Console.WriteLine("Invalid number. Press any key to return...");
                Console.ReadKey();
                return;
            }

            var task = tasks[number - 1];

            Console.WriteLine($"Current Title: {task.Title}");
            Console.Write("New Title (leave empty to keep): ");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                task.Title = newTitle;
            }

            Console.WriteLine($"Current Description: {task.Description}");
            Console.Write("New Description (leave empty to keep): ");
            string newDescription = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDescription))
            {
                task.Description = newDescription;
            }

            Console.WriteLine($"Current Due Date: {task.DueDate:yyyy-MM-dd}");
            Console.Write("New Due Date (yyyy-MM-dd, leave empty to keep): ");
            string newDueDateStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDueDateStr) &&
                DateTime.TryParse(newDueDateStr, out DateTime newDueDate))
            {
                task.DueDate = newDueDate;
            }

            Console.WriteLine($"Current Is Completed: {task.IsCompleted}");
            Console.Write("Is Completed? (y/n, leave empty to keep): ");
            string isCompletedStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(isCompletedStr))
            {
                task.IsCompleted = isCompletedStr.Trim().ToLower() == "y";
            }

            // Change category?
            Console.WriteLine($"Current Category: {task.Category?.Title} (Id: {task.CategoryId})");
            Console.Write("Change Category? (y/n): ");
            string changeCatStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(changeCatStr) &&
                changeCatStr.Trim().ToLower() == "y")
            {
                var categories = categoryContext.ReadAll();
                if (categories.Count == 0)
                {
                    Console.WriteLine("No categories found. Cannot change category.");
                }
                else
                {
                    Console.WriteLine("Available Categories:");
                    for (int i = 0; i < categories.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {categories[i].Title}");
                    }

                    int chosenIndex;
                    while (true)
                    {
                        Console.Write("Choose category by number: ");
                        if (int.TryParse(Console.ReadLine(), out chosenIndex) &&
                            chosenIndex >= 1 && chosenIndex <= categories.Count)
                        {
                            break;
                        }
                        Console.WriteLine("Invalid choice. Please try again.");
                    }

                    var selectedCategory = categories[chosenIndex - 1];
                    task.CategoryId = selectedCategory.Id;
                }
            }

            taskContext.Update(task);
            Console.WriteLine("Task updated successfully!");

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private static void PrintTasks(List<TaskItem> tasks)
        {
            Console.WriteLine("No.\tTitle\t\tDueDate\t\tCompleted\tCategory");
            Console.WriteLine("------------------------------------------------------------------");
            for (int i = 0; i < tasks.Count; i++)
            {
                var t = tasks[i];
                Console.WriteLine($"{i + 1}\t{t.Title}\t\t{t.DueDate:yyyy-MM-dd}\t{t.IsCompleted}\t\t{t.Category?.Title}");
            }
        }

        #endregion
    }
}
