How To Run:
1. Go to Directory where Docker file is placed. Run these 2 commands one by one.

docker build -t csharp-runner-api .

docker run --name codeapi-container -p 8080:8080 csharp-runner-api


HTTP Request Sample:
[POST] localhost:8080/run
{
  "code": "using System; class Solution { static void Main(){ Console.WriteLine(Console.ReadLine()); } }",
  "input": "hello World",
  "timeoutMs": 5000
}
