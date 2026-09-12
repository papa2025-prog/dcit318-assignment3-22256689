using System;
using System.Collections.Generic;
using System.IO;

namespace SchoolGradingSystem
{
    // a. Student Class
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Score { get; set; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80 && Score <= 100) return "A";
            if (Score >= 70 && Score <= 79) return "B";
            if (Score >= 60 && Score <= 69) return "C";
            if (Score >= 50 && Score <= 59) return "D";
            return "F";
        }
    }

    // b. Custom Exception: InvalidScoreFormatException
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    // c. Custom Exception: MissingFieldException
    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // d. StudentResultProcessor Class
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(',');
                    
                    // ii & v. Check fields length
                    if (parts.Length < 3)
                    {
                        throw new MissingFieldException($"Incomplete record found: '{line}'. Expected 3 fields.");
                    }

                    // iii & iv. Parse ID and Score
                    if (!int.TryParse(parts[0].Trim(), out int id))
                    {
                        throw new InvalidScoreFormatException($"Invalid ID format in line: '{line}'");
                    }

                    string fullName = parts[1].Trim();

                    if (!int.TryParse(parts[2].Trim(), out int score))
                    {
                        throw new InvalidScoreFormatException($"Score could not be parsed as an integer in line: '{line}'");
                    }

                    students.Add(new Student(id, fullName, score));
                }
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (var student in students)
                {
                    // Example format requirement: "Alice Smith (ID: 101): Score = 84, Grade = A"
                    writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "students_input.txt";
            string outputPath = "students_report.txt";

            // Create a dummy input file for testing purposes
            File.WriteAllText(inputPath, "101, Alice Smith, 84\n102, Bob Jones, 72\n103, Charlie Brown, 55");

            // e. Main Application Flow inside try-catch block
            try
            {
                StudentResultProcessor processor = new StudentResultProcessor();
                List<Student> studentList = processor.ReadStudentsFromFile(inputPath);
                processor.WriteReportToFile(studentList, outputPath);
                
                Console.WriteLine("Processing complete! Summary report generated successfully.");
                Console.WriteLine(File.ReadAllText(outputPath));
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: The specified input file was not found.");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"Score Format Error: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"Data Format Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unforeseen error occurred: {ex.Message}");
            }
        }
    }
}