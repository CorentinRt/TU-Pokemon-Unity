
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer les TU sur le reste et de les implémenter

        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
        // - un heal ne régénère pas plus que les HP Max
        // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
        // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type

        [Test]
        public void FightOneTurnWithTypeFireAndWaterNoShield()
        {
            Character salameche = new Character(100, 50, 0, 20, TYPE.FIRE);
            Character tiplouf = new Character(100, 60, 0, 200, TYPE.WATER);
            Fight f = new Fight(salameche, tiplouf);
            FireBall fireBall = new FireBall();
            WaterBlouBlou bloublou = new WaterBlouBlou();

            f.ExecuteTurn(fireBall, bloublou);

            Assert.That(TypeResolver.GetFactor(fireBall.Type, tiplouf.BaseType), Is.EqualTo(0.8f));
            Assert.That(TypeResolver.GetFactor(bloublou.Type, salameche.BaseType), Is.EqualTo(1.2f));
            Assert.That(salameche.CurrentHealth, Is.EqualTo(76));   // 20 * 1.2(water > fire) = 24 -> 100 - 24 = 76
            Assert.That(tiplouf.CurrentHealth, Is.EqualTo(60));    // 50 * 0.8(fire < water) = 40 -> 100 - 40 = 60
        }
        [Test]
        public void FightOneTurnWithTypeFireAndGrassNoShield()
        {
            Character salameche = new Character(100, 50, 0, 20, TYPE.FIRE);
            Character tortipouss = new Character(200, 60, 0, 200, TYPE.GRASS);
            Fight f = new Fight(salameche, tortipouss);
            FireBall fireBall = new FireBall();
            MagicalGrass magicalGrass = new MagicalGrass();

            f.ExecuteTurn(fireBall, magicalGrass);

            Assert.That(TypeResolver.GetFactor(fireBall.Type, tortipouss.BaseType), Is.EqualTo(1.2f));
            Assert.That(TypeResolver.GetFactor(magicalGrass.Type, salameche.BaseType), Is.EqualTo(0.8f));
            Assert.That(salameche.CurrentHealth, Is.EqualTo(44));   // 70 * 0.8(grass < fire) = 56 -> 100 - 56 = 44
            Assert.That(tortipouss.CurrentHealth, Is.EqualTo(140));    // 50 * 1.2(fire > grass) = 60 -> 200 - 60 = 140
        }
        [Test]
        public void FightOneTurnWithTypeWaterAndGrassNoShield()
        {
            Character tortipouss = new Character(100, 60, 0, 200, TYPE.GRASS);
            Character tiplouf = new Character(200, 50, 0, 20, TYPE.WATER);
            Fight f = new Fight(tiplouf, tortipouss);
            MagicalGrass magicalGrass = new MagicalGrass();
            WaterBlouBlou bloublou = new WaterBlouBlou();

            f.ExecuteTurn(bloublou, magicalGrass);

            Assert.That(TypeResolver.GetFactor(magicalGrass.Type, tiplouf.BaseType), Is.EqualTo(1.2f));
            Assert.That(TypeResolver.GetFactor(bloublou.Type, tortipouss.BaseType), Is.EqualTo(0.8f));
            Assert.That(tiplouf.CurrentHealth, Is.EqualTo(116));   // 70 * 1.2(grass > water) = 84 -> 200 - 84 = 116
            Assert.That(tortipouss.CurrentHealth, Is.EqualTo(84));    // 20 * 0.8(water < grass) = 16 -> 100 - 16 = 84
        }
        [Test]
        public void FightOneTurnWithTypeFireAndWaterShield()
        {
            Character salameche = new Character(100, 50, 30, 20, TYPE.FIRE);
            Character tiplouf = new Character(100, 60, 20, 200, TYPE.WATER);
            Fight f = new Fight(salameche, tiplouf);
            FireBall fireBall = new FireBall();
            WaterBlouBlou bloublou = new WaterBlouBlou();

            f.ExecuteTurn(fireBall, bloublou);

            Assert.That(TypeResolver.GetFactor(fireBall.Type, tiplouf.BaseType), Is.EqualTo(0.8f));
            Assert.That(TypeResolver.GetFactor(bloublou.Type, salameche.BaseType), Is.EqualTo(1.2f));
            Assert.That(salameche.CurrentHealth, Is.EqualTo(100));   // 20 * 1.2(water > fire) = 24 -> 100 - (24 - 30) = 106 (ramené à 100 car surplus d'armure ne heal pas)
            Assert.That(tiplouf.CurrentHealth, Is.EqualTo(80));    // 50 * 0.8(fire < water) = 40 -> 100 - (40 - 20) = 80
        }
        [Test]
        public void FightOneTurnWithTypeFireAndGrassShield()
        {
            Character salameche = new Character(100, 50, 30, 20, TYPE.FIRE);
            Character tortipouss = new Character(200, 60, 10, 200, TYPE.GRASS);
            Fight f = new Fight(salameche, tortipouss);
            FireBall fireBall = new FireBall();
            MagicalGrass magicalGrass = new MagicalGrass();

            f.ExecuteTurn(fireBall, magicalGrass);

            Assert.That(TypeResolver.GetFactor(fireBall.Type, tortipouss.BaseType), Is.EqualTo(1.2f));
            Assert.That(TypeResolver.GetFactor(magicalGrass.Type, salameche.BaseType), Is.EqualTo(0.8f));
            Assert.That(salameche.CurrentHealth, Is.EqualTo(74));   // 70 * 0.8(grass < fire) = 56 -> 100 - (56 - 30) = 74
            Assert.That(tortipouss.CurrentHealth, Is.EqualTo(150));    // 50 * 1.2(fire > grass) = 60 -> 200 - (60 - 10) = 150
        }
        [Test]
        public void FightOneTurnWithTypeWaterAndGrassShield()
        {
            Character tortipouss = new Character(100, 60, 10, 200, TYPE.GRASS);
            Character tiplouf = new Character(200, 50, 20, 20, TYPE.WATER);
            Fight f = new Fight(tiplouf, tortipouss);
            MagicalGrass magicalGrass = new MagicalGrass();
            WaterBlouBlou bloublou = new WaterBlouBlou();

            f.ExecuteTurn(bloublou, magicalGrass);

            Assert.That(TypeResolver.GetFactor(magicalGrass.Type, tiplouf.BaseType), Is.EqualTo(1.2f));
            Assert.That(TypeResolver.GetFactor(bloublou.Type, tortipouss.BaseType), Is.EqualTo(0.8f));
            Assert.That(tiplouf.CurrentHealth, Is.EqualTo(136));   // 70 * 1.2(grass > water) = 84 -> 200 - (84 - 20) = 136
            Assert.That(tortipouss.CurrentHealth, Is.EqualTo(94));    // 20 * 0.8(water < grass) = 16 -> 100 - (16 - 10) = 94
        }
        [Test]
        public void SameTypeFights()
        {
            WaterBlouBlou bloublou = new WaterBlouBlou();
            MagicalGrass magicalGrass = new MagicalGrass();
            FireBall fireBall = new FireBall();

            Character kaiminus = new Character(100, 60, 10, 200, TYPE.WATER);
            Character tiplouf = new Character(200, 50, 20, 20, TYPE.WATER);

            Character tortipouss = new Character(100, 60, 10, 200, TYPE.GRASS);
            Character majaspic = new Character(200, 50, 20, 20, TYPE.GRASS);

            Character ouisticram = new Character(100, 60, 10, 200, TYPE.FIRE);
            Character feunec = new Character(200, 50, 20, 20, TYPE.FIRE);

            Fight fWater = new Fight(kaiminus, tiplouf);
            Fight fGrass = new Fight(tortipouss, majaspic);
            Fight fFire = new Fight(ouisticram, feunec);

            fWater.ExecuteTurn(bloublou, bloublou); // dmg -> 20
            fGrass.ExecuteTurn(magicalGrass, magicalGrass); // dmg -> 70
            fFire.ExecuteTurn(fireBall, fireBall); // dmg -> 50

            // Water
            Assert.That(TypeResolver.GetFactor(bloublou.Type, kaiminus.BaseType), Is.EqualTo(1.0f));
            Assert.That(TypeResolver.GetFactor(bloublou.Type, tiplouf.BaseType), Is.EqualTo(1.0f));
            Assert.That(kaiminus.CurrentHealth, Is.EqualTo(90));
            Assert.That(tiplouf.CurrentHealth, Is.EqualTo(200));

            // Grass
            Assert.That(TypeResolver.GetFactor(magicalGrass.Type, tortipouss.BaseType), Is.EqualTo(1.0f));
            Assert.That(TypeResolver.GetFactor(magicalGrass.Type, majaspic.BaseType), Is.EqualTo(1.0f));
            Assert.That(tortipouss.CurrentHealth, Is.EqualTo(40));
            Assert.That(majaspic.CurrentHealth, Is.EqualTo(150));

            // Fire
            Assert.That(TypeResolver.GetFactor(fireBall.Type, ouisticram.BaseType), Is.EqualTo(1.0f));
            Assert.That(TypeResolver.GetFactor(fireBall.Type, feunec.BaseType), Is.EqualTo(1.0f));
            Assert.That(ouisticram.CurrentHealth, Is.EqualTo(60));
            Assert.That(feunec.CurrentHealth, Is.EqualTo(170));
        }

        [Test]
        public void HealOnceHealOverMaxPvHealAfterDeath()
        {
            Character pingoleon = new Character(400, 50, 20, 20, TYPE.WATER);

            Punch p = new Punch();

            pingoleon.ReceiveAttack(p); // pv = 350 -> 400 - (70 - 20)
            Assert.That(pingoleon.CurrentHealth, Is.EqualTo(350));

            pingoleon.Heal(20); // pv = 370
            Assert.That(pingoleon.CurrentHealth, Is.EqualTo(370));

            pingoleon.Heal(70); // pv = 400 (440 ramené à pv max)
            Assert.That(pingoleon.CurrentHealth, Is.EqualTo(400));

            MegaPunch mega = new MegaPunch();
            
            pingoleon.ReceiveAttack(mega);
            Assert.That(pingoleon.IsAlive, Is.EqualTo(false));

            pingoleon.Heal(40); // pv -> 0 (ne heal pas car mort)
            Assert.That(pingoleon.CurrentHealth, Is.EqualTo(0));
        }

        [Test]
        public void EquipementSpeedChangeOrderAttack()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character mewtwo = new Character(1000, 5000, 0, 200, TYPE.NORMAL);
            Equipment speedChanger = new Equipment(0, 0, 0, 8000);
            pikachu.Equip(speedChanger);
            Fight f = new Fight(pikachu, mewtwo);
            Punch p = new Punch();
            MegaPunch mp = new MegaPunch();

            f.ExecuteTurn(mp, mp);

            Assert.That(pikachu.IsAlive, Is.EqualTo(true));
            Assert.That(mewtwo.IsAlive, Is.EqualTo(false));
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(pikachu.MaxHealth));
            Assert.That(f.IsFightFinished, Is.EqualTo(true));
        }
        [Test]
        public void UnsequipEquipementSpeedLoose()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character mewtwo = new Character(1000, 5000, 60, 200, TYPE.NORMAL);
            Equipment speedChanger = new Equipment(0, 0, 0, 8000);
            pikachu.Equip(speedChanger);
            Fight f = new Fight(pikachu, mewtwo);
            Punch p = new Punch();
            MegaPunch mp = new MegaPunch();

            f.ExecuteTurn(p, p);    // pikachu tape en premier (voir test au dessus)

            Assert.That(pikachu.IsAlive, Is.EqualTo(true));
            Assert.That(mewtwo.IsAlive, Is.EqualTo(true));
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(60)); // pv = 60 -> 100 - (70 - 30)
            Assert.That(mewtwo.CurrentHealth, Is.EqualTo(990)); // pv = 990 -> 1000 - (70 - 60)
            Assert.That(f.IsFightFinished, Is.EqualTo(false));

            pikachu.Unequip();

            f.ExecuteTurn(mp, mp);  // pikachu ne tape plus en premier et va etre KO avant de taper mewtwo

            Assert.That(pikachu.IsAlive, Is.EqualTo(false));
            Assert.That(mewtwo.IsAlive, Is.EqualTo(true));
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(0));
            Assert.That(mewtwo.CurrentHealth, Is.EqualTo(990));
            Assert.That(f.IsFightFinished, Is.EqualTo(true));
        }

        [Test]
        public void HighDefenseDontHeal()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character raichu = new Character(200, 70, 0, 30, TYPE.NORMAL);

            Fight f = new Fight(pikachu, raichu);

            Punch p = new Punch();

            f.ExecuteTurn(p, p);

            Assert.That(pikachu.IsAlive, Is.EqualTo(true));
            Assert.That(raichu.IsAlive, Is.EqualTo(true));
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(60)); // pv = 60 -> 100 - (70 - 30)
            Assert.That(raichu.CurrentHealth, Is.EqualTo(130));  // pv = 130 -> 200 - 70

            Equipment megaShield = new Equipment(0, 0, 100, 0);

            raichu.Equip(megaShield);

            f.ExecuteTurn(p, p);

            Assert.That(pikachu.IsAlive, Is.EqualTo(true));
            Assert.That(raichu.IsAlive, Is.EqualTo(true));
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(20)); // pv = 20 -> 60 - (70 - 30)
            Assert.That(raichu.CurrentHealth, Is.EqualTo(130));  // pv = 130 -> 130 - (70 - 100) -> 130 - - 30 -> 160 (ramené à 130 car surplus armure ne peut pas heal)
        }

        [Test]
        public void DontFightIfFightIsOver()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character raichu = new Character(200, 70, 0, 30, TYPE.NORMAL);

            MegaPunch mp = new MegaPunch();

            Equipment decreaseSpeedEquipment = new Equipment(0, 0, 0, -20); // maintenant c pikachu qui tape en premier et pas raichu

            raichu.Equip(decreaseSpeedEquipment);

            Punch p = new Punch();

            Fight f = new Fight(pikachu, raichu);

            f.ExecuteTurn(p, mp);
            Assert.That(pikachu.IsAlive, Is.EqualTo(false));
            Assert.That(raichu.IsAlive, Is.EqualTo(true));
            Assert.That(raichu.CurrentHealth, Is.EqualTo(130));
            Assert.That(f.IsFightFinished, Is.EqualTo(true));

            f.ExecuteTurn(mp, mp);  // les Pokemon n'ataqueront pas car le combat est fini (donc raichu reste vivant)

            Assert.That(pikachu.IsAlive, Is.EqualTo(false));
            Assert.That(raichu.IsAlive, Is.EqualTo(true));
            Assert.That(raichu.CurrentHealth, Is.EqualTo(130));
            Assert.That(f.IsFightFinished, Is.EqualTo(true));
        }
    }
}
