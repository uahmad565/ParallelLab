using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Result
{
	
	
	public static List<int> myReverseArr(List<int> a)
	{
		a.Reverse();
		return a;
	}
    /*
     * Complete the 'reverseArray' function below.
     *
     * The function is expected to return an INTEGER_ARRAY.
     * The function accepts INTEGER_ARRAY a as parameter.
     */

    public static List<int> reverseArray(List<int> a)
    {
        //a.Reverse();
        return myReverseArr(a);
    }

}

class Solution
{
    public static void Main(string[] args)
    {
		var outputPath=@System.Environment.GetEnvironmentVariable("OUTPUT_PATH");
		//string outputPath = @"C:\Users\Usman\Desktop\output.txt";
        TextWriter textWriter = new StreamWriter(outputPath, false);
		
        int arrCount = Convert.ToInt32(Console.ReadLine().Trim());
        Console.WriteLine("arrCount: " + arrCount);
        List<int> arr = Console.ReadLine().TrimEnd().Split(' ').ToList().Select(arrTemp => Convert.ToInt32(arrTemp)).ToList();

        List<int> res = Result.reverseArray(arr);

        textWriter.WriteLine(String.Join(" ", res));
		
        textWriter.Flush();
        textWriter.Close();
    }
}
