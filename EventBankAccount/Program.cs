namespace EventBankAccount
{
    class Account
    {
        // Объявить делегат-обработчик и событие
        public delegate void AccountHandler(Account sender, AccountEventArgs e);
        public event AccountHandler? Notify;

        // Сумма на счете
        public int Sum { get; private set; }

        // В конструкторе устанавливаем начальную сумму на счете
        public Account(int sum) => Sum = sum;

        // Добавление средств на счет
        public void Put(int sum)
        {
            Sum += sum;
            Notify?.Invoke(this, new AccountEventArgs($"На счет поступило: {sum}", sum));
        }

        // Списание средств со счета
        public void Take(int sum)
        {
            if (Sum >= sum)
            {
                Sum -= sum;
                Notify?.Invoke(this, new AccountEventArgs($"Со счета снято: {sum}", sum));
            }
            else
            {
                Notify?.Invoke(this, new AccountEventArgs($"Недостаточно денег на счете. Текущий баланс: {sum}", sum));
            }
        }
    }

    class AccountEventArgs
    {
        // Сообщение
        public string Message { get; }

        // Сумма, на которую изменился счет
        public int Sum { get; }
        public AccountEventArgs(string message, int sum)
        {
            Message = message;
            Sum = sum;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("******* БАНКОМАТ *******\n");

            Account account = new Account(100);

            // Добавить обработчик для события Notify
            account.Notify += (s, e) =>
            {
                Console.WriteLine("---------------------------------------------------------------------------");
                Console.WriteLine($"Сумма транзакции: {e.Sum}");
                Console.WriteLine(e.Message);
                Console.WriteLine($"Текущая сумма на счете: {s.Sum}");
                Console.WriteLine("---------------------------------------------------------------------------");
            };

            account.Put(20); // добавить на счет 20
            Console.WriteLine($"Сумма на счете: {account.Sum}");
            account.Take(70);
            Console.WriteLine($"Сумма на счете: {account.Sum}");
            account.Take(180);
            Console.WriteLine($"Сумма на счете: {account.Sum}");

            Console.ReadLine();
        }
    }
}