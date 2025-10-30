using FluentAssertions;
using FluentAssertions.Equivalency;
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

        actualTsar.Should().BeEquivalentTo(expectedTsar, options => options
                .Excluding((IMemberInfo memberInfo) => memberInfo.Name == nameof(Person.Id) 
                                                       && memberInfo.DeclaringType == typeof(Person))
                .IgnoringCyclicReferences()
                .AllowingInfiniteRecursion());
        // Решение через FluidAssertions 
        // 1. Лучше читается, сразу видно что assert проверяет на полное соответствие всех полей класса кроме всех Id у класса Person
        // 2. Теперь поддерживает лёгкое расширение, если добавим новое поле которое требует проверки то не нужно будет переписывать assert,
        // если добавим поле которое не требует проверки то необходимо будет добавить его имя в Excluding()
        // 3. Поддерживает длинные цепочки родителей за счёт AllowingInfiniteRecursion(),
        // иначе на 10(по дефолту в FluentAssertions) рекурсивном вызове в глубину проверка бы остановилась
        // 4. Останавливается когда обнаруживает что проверка поля производилась раньше благодаря IgnoringCyclicReferences(),
        // таким образом не проваливается в бесконечную рекурсию
        // 5. При ошибке действительно показывает какое поле не совпало


    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));
        // Какие недостатки у такого подхода? 
        // 1. Он нерасширяем, при добавлении нового поля придётся переписывать AreEqual
        // 2. Слабая читаемость, если в методе AreEqual допущена ошибка нужно смотреть на каждое поле которое он сравнивает чтобы найти ошибку
        // 3. Возможна бесконечная рекурсия если parent образует цикл
        // 4. При ошибке не показывает какие поля не совпадают, только что AreEqual вернуло False
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
