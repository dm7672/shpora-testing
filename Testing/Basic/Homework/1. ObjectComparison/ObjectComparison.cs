using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("Refactored")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Перепишите код на использование Fluent Assertions.
        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options.Excluding(person => person.Id)
                   .Excluding(person => person.Parent.Id)
            );
        // Недостатки подхода CustomEquality по сравнению с решением выше в том что
        // 1. Используется проверка на истинность AreEqual, в случае ошибки покажет только то что AreEqual вернуло False
        // 2. Кастомный Ассерт ниже нерасширяем, при попытке добавить новое поле в класс придётся переписывать его
        // 3. Читаемость гораздо хуже, в ассерте могла быть пропущена проверка поля и внешне он выглядел бы также,
        // а значит чтобы понять что пытаемся проверить пришлось бы читать весь код
        // 4. Код выше короче
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
