using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UglyTrivia
{
    class Questions
    {
        public static void AddLast01(LinkedList<string> popQuestions, int i)
        {
            popQuestions.AddLast("Pop Question " + i);
        }

        public static void AskQuestion01(LinkedList<string> popQuestions)
        {
            Console.WriteLine(popQuestions.First());
            popQuestions.RemoveFirst();
        }
    }
    
    public class Game
    {
        private readonly List<string> _players = new List<string>();
        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];
        private readonly bool[] _inPenaltyBox = new bool[6];
        private int _currentPlayer = 0;
        private bool _isGettingOutOfPenaltyBox;

        private LinkedList<string> _popQuestions = new LinkedList<string>();
        private LinkedList<string> _scienceQuestions = new LinkedList<string>();
        private LinkedList<string> _sportsQuestions = new LinkedList<string>();
        private LinkedList<string> _rockQuestions = new LinkedList<string>();

        public Game()
        {
            for (var i = 0; i < 50; i++)
            {
                Questions.AddLast01(_popQuestions, i);
                _scienceQuestions.AddLast("Science Question " + i);
                _sportsQuestions.AddLast("Sports Question " + i);
                _rockQuestions.AddLast(CreateRockQuestion(i));
            }
        }

        public string CreateRockQuestion(int index) => "Rock Question " + index;

        public bool IsPlayable() => (HowManyPlayers() >= 2);

        public bool Add(String playerName)
        {
            _players.Add(playerName);
            _places[HowManyPlayers()] = 0;
            _purses[HowManyPlayers()] = 0;
            _inPenaltyBox[HowManyPlayers()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
            return true;
        }

        public int HowManyPlayers() => _players.Count;

        public void Roll(int roll)
        {
            Console.WriteLine(_players[_currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    _isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(_players[_currentPlayer] + " is getting out of the penalty box");
                    _places[_currentPlayer] = _places[_currentPlayer] + roll;
                    if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

                    Console.WriteLine(_players[_currentPlayer]
                            + "'s new location is "
                            + _places[_currentPlayer]);
                    Console.WriteLine("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(_players[_currentPlayer] + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }

            }
            else
            {

                _places[_currentPlayer] += roll;
                if (_places[_currentPlayer] > 11) _places[_currentPlayer] -= 12;

                Console.WriteLine(_players[_currentPlayer]
                        + "'s new location is "
                        + _places[_currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == "Pop")
            {
                Questions.AskQuestion01(_popQuestions);
            }
            if (CurrentCategory() == "Science")
            {
                Console.WriteLine(_scienceQuestions.First());
                _scienceQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Sports")
            {
                Console.WriteLine(_sportsQuestions.First());
                _sportsQuestions.RemoveFirst();
            }
            if (CurrentCategory() == "Rock")
            {
                Console.WriteLine(_rockQuestions.First());
                _rockQuestions.RemoveFirst();
            }
        }

        private string CurrentCategory()
        {
            switch (_places[_currentPlayer])
            {
                case 0:
                    return "Pop";
                case 1:
                    return "Science";
                case 2:
                    return "Sports";
                case 4:
                    return "Pop";
                case 5:
                    return "Science";
                case 6:
                    return "Sports";
                case 8:
                    return "Pop";
                case 9:
                    return "Science";
                case 10:
                    return "Sports";
                default:
                    return "Rock";
            }
        }

        public bool wasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    return GettingOutOfPenaltyBoxProcess();
                }

                _currentPlayer++;
                if (_currentPlayer == _players.Count) _currentPlayer = 0;
                return true;
            }
            else
            {
                return NotInPenaltyBox();
            }
        }

        private bool NotInPenaltyBox()
        {
            Console.WriteLine("Answer was correct!!!!");
            _purses[_currentPlayer]++;
            Console.WriteLine(_players[_currentPlayer]
                              + " now has "
                              + _purses[_currentPlayer]
                              + " Gold Coins.");

            bool winner = DidPlayerWin();
            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;

            return winner;
        }

        private bool GettingOutOfPenaltyBoxProcess()
        {
            Console.WriteLine("Answer was correct!!!!");
            _purses[_currentPlayer]++;
            Console.WriteLine(_players[_currentPlayer]
                              + " now has "
                              + _purses[_currentPlayer]
                              + " Gold Coins.");

            bool winner = DidPlayerWin();
            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;

            return winner;
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;
            return true;
        }

        private bool DidPlayerWin() => _purses[_currentPlayer] != 6;
    }

}
