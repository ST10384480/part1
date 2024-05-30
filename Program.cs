using System;
using System.Collections.Generic;
using System.Linq;

// Represents an ingredient with name, quantity, unit, calories, and food group.
class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; }
    public double Calories { get; set; }
    public string FoodGroup { get; set; }
}

// Represents a step in a recipe with a description.
class RecipeStep
{
    public string Description { get; set; }
}

// Represents a recipe with a name, list of ingredients, and steps.
class Recipe
{
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<RecipeStep> Steps { get; set; }
    public double TotalCalories => Ingredients.Sum(ingredient => ingredient.Calories * ingredient.Quantity);

    // Delegate to notify when total calories exceed a threshold.
    public delegate void CaloriesExceededHandler(double totalCalories);
    public event CaloriesExceededHandler OnCaloriesExceeded;

    public Recipe()
    {
        Ingredients = new List<Ingredient>();
        Steps = new List<RecipeStep>();
    }

    // Enters recipe details, including ingredients and steps.
    public void EnterRecipeDetails()
    {
        Console.WriteLine($"Entering details for recipe: {Name}");

        Console.Write("Enter the number of ingredients: ");
        int ingredientCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < ingredientCount; i++)
        {
            Console.WriteLine($"Enter details for ingredient #{i + 1}:");
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Quantity: ");
            double quantity;
            while (!double.TryParse(Console.ReadLine(), out quantity))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for Quantity:");
            }

            Console.Write("Unit of measurement: ");
            string unit = Console.ReadLine();

            Console.Write("Calories per unit: ");
            double calories;
            while (!double.TryParse(Console.ReadLine(), out calories))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for Calories per unit:");
            }

            Console.Write("Food group: ");
            string foodGroup = Console.ReadLine();

            Ingredients.Add(new Ingredient { Name = name, Quantity = quantity, Unit = unit, Calories = calories, FoodGroup = foodGroup });
        }

        Console.Write("Enter the number of steps: ");
        int stepCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < stepCount; i++)
        {
            Console.WriteLine($"Enter description for step #{i + 1}:");
            string description = Console.ReadLine();
            Steps.Add(new RecipeStep { Description = description });
        }

        if (TotalCalories > 300)
        {
            OnCaloriesExceeded?.Invoke(TotalCalories);
        }
    }

    // Displays the recipe details, including ingredients and steps.
    public void DisplayRecipe()
    {
        Console.WriteLine($"\nRecipe: {Name}");
        Console.WriteLine("Ingredients:");
        foreach (var ingredient in Ingredients)
        {
            Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} {ingredient.Name} ({ingredient.Calories} calories per unit, {ingredient.FoodGroup})");
        }

        Console.WriteLine("\nSteps:");
        for (int i = 0; i < Steps.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Steps[i].Description}");
        }

        Console.WriteLine($"\nTotal Calories: {TotalCalories}");
    }

    // Scales the recipe by a given factor.
    public void ScaleRecipe(double factor)
    {
        foreach (var ingredient in Ingredients)
        {
            ingredient.Quantity *= factor;
        }
    }

    // Clears the recipe data.
    public void ClearData()
    {
        Ingredients.Clear();
        Steps.Clear();
    }
}

class Program
{
    static List<Recipe> recipes = new List<Recipe>();

    static void Main(string[] args)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nRecipe Management System");
            Console.WriteLine("1. Add a new recipe");
            Console.WriteLine("2. Display a recipe");
            Console.WriteLine("3. List all recipes");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid option number:");
            }

            switch (choice)
            {
                case 1:
                    AddRecipe();
                    break;
                case 2:
                    DisplayRecipe();
                    break;
                case 3:
                    ListRecipes();
                    break;
                case 4:
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
    static void AddRecipe()
    {
        Console.Write("Enter the name of the recipe: ");
        string name = Console.ReadLine();

        Recipe recipe = new Recipe { Name = name };
        recipe.OnCaloriesExceeded += totalCalories => Console.WriteLine($"Warning: Total calories ({totalCalories}) exceed 300!");

        recipe.EnterRecipeDetails();
        recipes.Add(recipe);
        recipes = recipes.OrderBy(r => r.Name).ToList();
    }
    static void DisplayRecipe()
    {
        ListRecipes();

        if (recipes.Count == 0)
        {
            Console.WriteLine("No recipes available to display.");
            return;
        }

        Console.Write("Enter the recipe number to display: ");
        int recipeNumber;
        while (!int.TryParse(Console.ReadLine(), out recipeNumber) || recipeNumber < 1 || recipeNumber > recipes.Count)
        {
            Console.WriteLine("Invalid input. Please enter a valid recipe number:");
        }

        recipes[recipeNumber - 1].DisplayRecipe();
    }

    static void ListRecipes()
    {
        if (recipes.Count == 0)
        {
            Console.WriteLine("No recipes available.");
            return;
        }

        Console.WriteLine("Recipes:");
        for (int i = 0; i < recipes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {recipes[i].Name}");
        }
    }
}
//REFERENCES:
//(https://nunit.org/)
//(https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)[DATE ACCESSED : 30 APRIL 2024]
//(https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/)[DATE ACCESSED : 28 MAY 2024]
//](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/classes)[DATE ACCESSED : 17 MAY 2024]
//https://www.w3schools.com/cs/cs_properties.php [DATE ACCESSED : 29 APRIL 2024]
//https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/using-properties  [DATE ACCESSED : 12 MAY 2024]
//https://learn.microsoft.com/en-us/dotnet/api/system.console.writeline?view=net-8.0  [DATE ACCESSED : 13 MAY 2024]
//https://stackoverflow.com/questions/3139118/how-to-initialize-a-list-of-strings-liststring-with-many-string-values  [DATE ACCESSED : 15 MAY 2024]