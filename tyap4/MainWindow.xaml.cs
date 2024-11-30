using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tyap4
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		int StatesNum = 0;
		int AlphabetLength = 0;
		string[] AlphabetArray = new string[0];
		private List<TextBox> RulesDict = new List<TextBox>();
		List<string> FirstStageRule = new List<string>();
		List<string> AlphabetRule = new List<string>();
		bool s1 = false;
		bool s2 = false;
		List<string> NonTerminalSymb = new List<string>();
		List<string> TerminalSymb = new List<string>();
		List<string> AllString = new List<string>();


		private List<List<string>> Rules = new List<List<string>>();
		private string EmptyString = "Z";
		private const string DeleteFromStackSymbol = "-";
		private const string IgnoreStack = "_";
		private const string AddToStack = "+";
		private string SpecialSymbolSolDelim = "\n";
		private string Chain = "";
		private string StartStage = "";
		private string FinalStage = "";
		private string CurrentStage = "";
		private string Stack = "Z";
		private string TranslatedString = "";
		private string IgnoreString = "_";


		public MainWindow()
		{
			InitializeComponent();
		}
		private void FileInputButton_Click(object sender, RoutedEventArgs e)
		{
			ST1.Height = 0;
			ST3.Height = 30;
			//вызвать функции для считывания из файла и выведения на экран

			string fileStatus = ReadFromFile("l.txt");
			Console.WriteLine(fileStatus);
			if (fileStatus == "ok")
			{
				//файл считан успешно

				/*Console.WriteLine(ChainCheck());*/


			}


		}

		private void CalcButton_Click(object sender, RoutedEventArgs e)
		{





			Chain = CheckStringInput.Text;

			string outputString = ChainCheck();
			Output.Content = outputString;
/*			var labelVar = new TextBlock { Width = 800 };
			labelVar.Text = outputString;
			ScrollViewer scrollViewer = new ScrollViewer
			{
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
				Content = labelVar,
				Height = 200
			};
			MainStackPanel.Children.Add(scrollViewer);*/

		}

		private string ReadFromFile(string filename)
		{

			string[] lines;
			try
			{
				lines = File.ReadAllLines(filename);
				foreach (string line in lines)
				{
					Console.WriteLine(line);
					string[] parts = line.Split(' ');

					Rules.Add(new List<string> { parts[0].ToString(), parts[1].ToString(), parts[2].ToString(), parts[3].ToString(), parts[4].ToString(), parts[5].ToString() });


				}
			}
			catch (IOException e)
			{
				Console.WriteLine("Ошибка при чтении файла:");
				Console.WriteLine(e.Message);
				return "ошибка при чтении файла";

			}





			return "ok";
		}



		private string ChainCheck()
		{
			StartStage = Rules[0][0];
			CurrentStage = StartStage;
			FinalStage = Rules[Rules.Count - 1][3];
			Stack = "Z";
			Chain = Chain + "Z";
			TranslatedString = "";
			string changes = "";
			changes += " (" + CurrentStage + " " + Chain + " " + Stack + ") " + "\n";
			int chainLength = Chain.Length;
			for (int i = 0; i < chainLength; i++)
			{
				bool flag = true;

				//начинаются пляски с проверками...
				foreach (List<string> rule in Rules)
				{
					Console.WriteLine(CurrentStage + " == " + rule[0].ToString());
					if (rule[0] == CurrentStage)
					{
						Console.WriteLine(Chain[0] + " == " + rule[1].ToString());
						if (rule[1] == Chain[0].ToString())
						{
							Console.WriteLine(Stack[0] + " == " + rule[2]);
							if (rule[2] == "Z" && Stack == "Z") // пустой стек
							{
								Console.WriteLine("что-то происходит с действием " + rule[4]);
								Console.WriteLine("замена " + Stack + " на " + Stack.Remove(0, 1));
								if (rule[4] == DeleteFromStackSymbol && rule[3] != FinalStage)
								{
									changes += "невозможно удалить из пустого стека символ в " + CurrentStage ;
									break;
								}
								switch (rule[4])
								{

									case IgnoreStack:
										break;
									case AddToStack:
										Stack = rule[1] + Stack;
										break;
									case DeleteFromStackSymbol:
										Stack = Stack.Remove(0, 1);
										break;
								}
								flag = false;
								Console.WriteLine("замена " + CurrentStage + " на " + rule[3]);
								CurrentStage = rule[3];
								Console.WriteLine("замена " + Chain + " на " + Chain.Remove(0, 1));
								if (rule[5] != IgnoreString)
								{
									TranslatedString += rule[5];
								}
								Chain = Chain.Remove(0, 1);
								if(Stack.Length!=0 && Chain.Length == 0 && CurrentStage != FinalStage)
								{
									Chain = "Z";
									i = i - 1;
								}
								break;
							}
							else
							{
								Console.WriteLine("что-то происходит с действием " + rule[4]);
								if (Stack.Length != 1 && rule[2] == Stack[0].ToString()) // не пустой стек
								{

									if (rule[4] == DeleteFromStackSymbol && rule[3] != FinalStage && Stack.Length == 1)
									{
										changes += "невозможно удалить из пустого стека символ в " + CurrentStage;
										break;
									}

									Console.WriteLine("замена " + Stack + " на " + Stack.Remove(0, 1));
									switch (rule[4])
									{

										case IgnoreStack:
											break;
										case AddToStack:
											Stack = rule[1] + Stack;
											break;
										case DeleteFromStackSymbol:
											Stack = Stack.Remove(0, 1);
											break;
									}
									Console.WriteLine("замена " + CurrentStage + " на " + rule[3]);
									CurrentStage = rule[3];
									Console.WriteLine("замена " + Chain + " на " + Chain.Remove(0, 1));
									flag = false;
									if (rule[5] != IgnoreString)
									{
										TranslatedString += rule[5];
									}
									Chain = Chain.Remove(0, 1);
									if (Stack.Length != 0 && Chain.Length == 0 && CurrentStage != FinalStage)
									{
										Chain = "Z";
										i = i - 1;
									}
									break;
								}

							}
						}

					}

				}
				

				changes += " (" + CurrentStage + " " + Chain + " " + Stack + " " + TranslatedString + ") " + "\n";

				if (flag && CurrentStage != FinalStage)
				{
					if(Chain.Length!=0 && Stack.Length != 0)
					{
						return changes + " нет правила для " + " (" + CurrentStage + " " + Chain[0] + " " + Stack[0] + ") ";

					} else
					{
						return changes + " нет правила для " + " (" + CurrentStage + ") ";
					}
				}
				Console.WriteLine(changes);
				if (Chain.Length == 0)
				{
					break;
				}
			}
			//костыльная проверка на конец
			/*            if (Chain.Length == 0 && Rules[Rules.Count - 1][1] == "_") 
						{
							CurrentStage = FinalStage;
						}*/
			foreach (var r in Rules)
			{
				if (r[4] == FinalStage)
				{
					if (r[0] == CurrentStage && r[2] == Stack[0].ToString())
					{
						if (Chain.Length == 0 && r[1] == EmptyString)
						{
							break;
						}
					}
				}
			}

			if (CurrentStage != FinalStage)
			{
				changes += SpecialSymbolSolDelim + CurrentStage + " автомат не в финальном состоянии, должно быть " + FinalStage;
				return changes;
			}
			else
			{
				changes += " успешно завершено";

				return changes;
			}

		}


	}
}
