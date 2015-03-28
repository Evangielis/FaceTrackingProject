using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF_FaceTracking.cs
{
    public enum Difficulty
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    };

    public struct Question
    {
        public int id;
        public string text;
        public Difficulty diff;
        public string answer;

        public Question(int id, string text, Difficulty diff, string answer)
        {
            this.id = id;
            this.text = text;
            this.diff = diff;
            this.answer = answer;
        }

        public Question(int id, string text, int diff, string answer) : this(id, text, (Difficulty)diff, answer) { }
    }

    class Quiz : IEnumerable<Question>
    {
        const int DEFAULT_NUM_QUESTIONS = 15;

        public string Owner { get; private set; }
        QuizDataSet that;
        public Stack<Question> qBag;
        public int NumQuestions { get; private set; }

        public Quiz(string Owner, int numQuestions)
        {
            this.Owner = Owner;
            this.NumQuestions = numQuestions;
            this.that = new QuizDataSet();
            this.qBag = new Stack<Question>();

            this.LoadQuestions();
            this.InitializeBag();
        }

        public Quiz(string Owner) : this(Owner, DEFAULT_NUM_QUESTIONS) {}

        public Question GetQuestion()
        {            
            return this.qBag.Pop();
        }
        
        private void InitializeBag()
        {
            foreach (DataRow dr in that.Tables["QUESTIONS"].Rows)
            {
                qBag.Push(QuestionFromRow(dr));
            }
        }

        private Question QuestionFromRow(DataRow dr)
        {
            int id = Convert.ToInt32(dr["pId"]);
            string text = (string)dr["Text"];
            int diff = Convert.ToInt32(dr["Difficulty"]);
            string ans = (string)dr["Answer"];
            return new Question(id, text, diff, ans);
        }

        private void PrintAllQuestions()
        {
            foreach (DataRow dr in that.Tables["QUESTIONS"].Rows)
            {
                Console.Out.WriteLine(dr["Text"] + " " + dr["Difficulty"]);
            }
        }

        private void LoadQuestions()
        {
            //Console.Out.WriteLine(that.DataSetName);
            AddQuestion(1, "I have two coins totaling 15 cents, one of which is not a nickle. What are the two coins?", Difficulty.Easy, "a dime and a nickle.");
            AddQuestion(2, "You have two ropes. Each rope takes one hour to burn. These ropes are not identical, nor are they uniform (i.e. it does not burn at an even rate and are different lengths). With only these two ropes and a way to light them, how do you measure 45 minutes?", Difficulty.Easy, "Light both ends of one rope, and only one end of the other rope. This will cause the first rope to burn out in 30 minutes. When the first one is burnt out, light the other end of the remaining rope and it will burn out in 15 minutes, measuring 45 minutes.");
            AddQuestion(3, "Find the only three words in the English language that are all of the following: \n1) Four letter long \n2) Start with T,C, and B \n3) The last three letters of each word are exactly the same \n4) They do not rhyme", Difficulty.Medium, "Tomb,comb, bomb");
            AddQuestion(4, "By myself i am 24, with a friend I am 20, and one more makes me dirty.", Difficulty.Hard, "The letter x");
            AddQuestion(5, "I never was, am always to be; no one has seen me, nor will they see me. Close to sun’s set, and far from sunrise; I will live on, till time’s own demise.", Difficulty.Medium, "Tomorrow");
            AddQuestion(6, "Kick me out of hate, I am past tense then. Put me back in the air, I’m a part of you. What am I?", Difficulty.Hard, "The letter H");
            AddQuestion(7, "Though I live beneath a roof, I never seem dry. If you will only hold me, I swear I will not lie.", Difficulty.Medium, "Your tongue");
            AddQuestion(8, "What goes on four legs in the morning, on two legs at noon, and on three legs in the evening?", Difficulty.Easy, "Man");
            AddQuestion(9, "There’s a dead man in a room surrounded by 53 bicycles. Why is he dead?", Difficulty.Hard, "He was caught cheating in the card game");
            AddQuestion(10, "What starts and ends with ‘e’, has only one letter in the middle, but still contains hundreds of words?", Difficulty.Easy, "Envelope");
            AddQuestion(11, "I fly when I am born, I lay when I’m alive, and I run when I’m dead. What am I?", Difficulty.Medium, "Snow");
            AddQuestion(12, "Find a word that the first 2 letters are a male, the first 3 letters are a female, the first four letters are a great male, and the whole word is a great female.", Difficulty.Hard, "Heroine");
            AddQuestion(13, "I have deserts without sand, I have seas without water, I have forests without trees. What am I?", Difficulty.Easy, "A map");
            AddQuestion(14, "Useless alone, but three is too much. Need two for a hike, and wooden if Dutch.", Difficulty.Hard, "Shoes");
            AddQuestion(15, "Is it always true that 6/2(1+2) = 6/2c where c=1+2?", Difficulty.Medium, "Maybe");
        }


        private void AddQuestion(int id, string text, Difficulty diff, string ans)
        {
            DataRow row = that.Tables["QUESTIONS"].NewRow();
            row["pID"] = id;
            row["Text"] = text;
            row["Difficulty"] = (int)diff;
            row["Answer"] = ans;
            that.Tables["QUESTIONS"].Rows.Add(row);
        }

        IEnumerator<Question> GetEnum()
        {
            Random rgen = new Random();
            return this.qBag.OrderBy(x => rgen.Next(this.NumQuestions)).GetEnumerator();
        }

        IEnumerator<Question> IEnumerable<Question>.GetEnumerator()
        {
            return this.GetEnum();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnum();
        }
    }
}
