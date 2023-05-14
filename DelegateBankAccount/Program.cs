namespace DelegateBankAccount
{
    // Объявляем делегат-обработчик
    public delegate void AccountHandler(string message);
    public class Account
    {
        int sum; // переменная для хранения суммы
        AccountHandler? taken; // переменная делегата

        // Через конструктор устанавливается начальная сумма на карте
        public Account(int sum) => this.sum = sum;

        // Регистрируем делегат
        public void RegisterHandler(AccountHandler del) => taken += del;

        // Отмена регистрации делегата
        public void UnregisterHandler(AccountHandler del) => taken -= del;

        // Добавить средства на счет
        public void Add(int sum) => this.sum += sum;

        // Взять средства со счета
        public void Take(int sum)
        {
            // Берем деньги, если на счете достаточно средств
            if (this.sum >= sum)
            {
                this.sum -= sum;
                // Вызываем делегат, передавая ему сообщение
                taken?.Invoke($"Со счета списано {sum} у.е.");
            }
            else taken?.Invoke($"Недостаточно средств. Баланс: {this.sum} у.е.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("******* БАНКОМАТ *******\n");

            // Создаем банковский счет
            Account account = new Account(200);

            // Добавляем в делегат ссылку на методы
            account.RegisterHandler(PrintSimpleMessage);
            account.RegisterHandler(PrintColorMessage);

            // Два раза подряд пытаемся снять деньги
            account.Take(100);
            account.Take(150);

            // Удаляем делегат
            account.UnregisterHandler(PrintColorMessage);

            // Вновь пытаемся снять деньги
            account.Take(50);

            Console.ReadLine();

            void PrintSimpleMessage(string message) => Console.WriteLine(message);
            void PrintColorMessage(string message)
            {
                // Устанавливаем красный цвет символов
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                // Сбрасываем настройки цвета
                Console.ResetColor();
            }
        }
    }
}