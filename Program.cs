using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    // Класс Оружие (без прочности)
    public class Weapon
    {
        public string Name { get; }
        public int Damage { get; }

        public Weapon(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }
    }

    // Класс Аптечка
    public class Aid
    {
        public string Name { get; }
        public int HealAmount { get; }

        public Aid(string name, int healAmount)
        {
            Name = name;
            HealAmount = healAmount;
        }
    }

    // Класс Противник
    public class Enemy
    {
        public string Name { get; }
        public int Health { get; set; }
        public int MaxHealth { get; }
        public Weapon Weapon { get; }

        public Enemy(string name, int health, Weapon weapon)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Weapon = weapon;
        }

        public int Attack()
        {
            return Weapon.Damage;
        }
    }

    // Класс Игрок
    public class Player
    {
        public string Name { get; }
        public int Health { get; set; }
        public int MaxHealth { get; }
        public Aid AidKit { get; }
        public Weapon Weapon { get; }
        public int Score { get; set; }

        public Player(string name, int health, Weapon weapon, Aid aidKit)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Weapon = weapon;
            AidKit = aidKit;
            Score = 0;
        }

        public int Attack()
        {
            return Weapon.Damage;
        }

        public void Heal()
        {
            Health += AidKit.HealAmount;
            if (Health > MaxHealth) Health = MaxHealth;
        }
    }

    class Program
    {
        static Random rnd = new Random();
        static List<string> enemyNames = new List<string> { "Варвар", "Гоблин", "Скелет", "Орк", "Тролль", "Зомби" };
        static List<string> weaponNames = new List<string> { "Меч", "Топор", "Булава", "Кинжал", "Копье", "Посох" };
        static List<string> aidNames = new List<string> { "малая", "средняя", "большая" };

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать, воин!");
            Console.WriteLine("Назови себя:");
            Console.Write("> ");
            string playerName = Console.ReadLine();

            // Создание игрока со случайными предметами
            Weapon playerWeapon = GenerateWeapon();
            Aid playerAid = GenerateAid();

            Player player = new Player(
                playerName,
                100,
                playerWeapon,
                playerAid
            );

            Console.WriteLine($"\nВаше имя **{player.Name}**!");
            Console.WriteLine($"Вам был ниспослан меч **{player.Weapon.Name} ({player.Weapon.Damage})**, а также **{player.AidKit.Name}** аптечка ({player.AidKit.HealAmount}hp).");
            Console.WriteLine($"У вас {player.Health}hp.\n");

            // Главный игровой цикл
            while (player.Health > 0)
            {
                Enemy enemy = GenerateEnemy();

                Console.WriteLine($"**{player.Name}** встречает врага **{enemy.Name} ({enemy.Health}hp)**, у врага на поясе сияет оружие **{enemy.Weapon.Name} ({enemy.Weapon.Damage})**");

                // Бой с текущим врагом
                while (enemy.Health > 0 && player.Health > 0)
                {
                    Console.WriteLine("Что вы будете делать?");
                    Console.WriteLine("1. Ударить");
                    Console.WriteLine("2. Пропустить ход");
                    Console.WriteLine("3. Использовать аптечку");
                    Console.Write("> ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": // Атака
                            int playerDamage = player.Attack();
                            enemy.Health -= playerDamage;
                            Console.WriteLine($"\n**{player.Name}** ударил противника **{enemy.Name}**");
                            break;

                        case "2": // Пропуск хода
                            Console.WriteLine($"\n**{player.Name}** пропускает ход");
                            break;

                        case "3": // Лечение
                            player.Heal();
                            Console.WriteLine($"\n**{player.Name}** использовал аптечку");
                            break;

                        default:
                            Console.WriteLine("\nНеверный выбор, пропускаем ход");
                            break;
                    }

                    // Если враг еще жив, он атакует
                    if (enemy.Health > 0)
                    {
                        int enemyDamage = enemy.Attack();
                        player.Health -= enemyDamage;
                        Console.WriteLine($"Противник **{enemy.Name}** ударил вас!");
                    }

                    // Показываем текущее состояние
                    Console.WriteLine($"У противника {Math.Max(enemy.Health, 0)}hp, у вас {Math.Max(player.Health, 0)}hp");

                    // Если игрок умер
                    if (player.Health <= 0)
                    {
                        Console.WriteLine("\nВы погибли...");
                        break;
                    }

                    // Если враг умер
                    if (enemy.Health <= 0)
                    {
                        Console.WriteLine($"\n**{player.Name}** победил врага **{enemy.Name}**!");
                        player.Score += 10;
                        Console.WriteLine($"+10 очков! Всего очков: {player.Score}\n");
                        break;
                    }

                    Console.WriteLine();
                }

                if (player.Health <= 0) break;

                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }

            Console.WriteLine("\n=== Игра окончена ===");
            Console.WriteLine($"Ваш счет: {player.Score} очков");
            Console.WriteLine("Спасибо за игру!");
        }

        static Enemy GenerateEnemy()
        {
            string name = enemyNames[rnd.Next(enemyNames.Count)];
            int health = rnd.Next(30, 71);
            Weapon weapon = GenerateWeapon();

            return new Enemy(name, health, weapon);
        }

        static Weapon GenerateWeapon()
        {
            string name = weaponNames[rnd.Next(weaponNames.Count)];
            int damage = rnd.Next(5, 16);

            return new Weapon(name, damage);
        }

        static Aid GenerateAid()
        {
            string name = aidNames[rnd.Next(aidNames.Count)];
            int healAmount = name switch
            {
                "малая" => 10,
                "средняя" => 25,
                "большая" => 50,
                _ => 10
            };

            return new Aid(name, healAmount);
        }
    }
}