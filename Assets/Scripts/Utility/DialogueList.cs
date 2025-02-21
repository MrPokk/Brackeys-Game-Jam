using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public static class DialogueList
{

    private static Dictionary<Type, List<string>> DialogueAll = new Dictionary<Type, List<string>>()
    {
        {
            typeof(Eldar), new List<string>()
            {
                "Hello, young alchemist. I’ve heard that excellent potions are brewed here. I need a special potion for my plants. They’ve completely withered.",
                "Hey, alchemist! Your potions are famous. My plants are dying! Need something to revive them.",
                "Hello, alchemist. I understand you create excellent potions. I’m in need of one for my plants, as they are not thriving. Maybe you have a potion that solves this problem?"
            }
        },
        {
            typeof(Grail), new List<string>()
            {
                "I Mr.Grail. I need a healing potion.",
                "Mr.Grail here. I need a potion to heal me.",
                "Im Mr.Grail, and I require a healing potion"
            }
        },
        {
            typeof(Grimbold), new List<string>()
            {
                "Potions. Strength, urgently",
                "I don't have enough strength"
            }
        },
        {
            typeof(Isabella), new List<string>()
            {
                "Greetings, lord of the cauldron! I've heard that you can make potions as well as Merlin himself! I have a business proposal for you, you might need some ingredients, look what I have.",
                "Hail, potion-brewing extraordinaire! They say you’re the modern-day Merlin! I’ve got a business proposition that’s practically magical. Speaking of magic, you might need some ingredients… and lucky you, I brought some!"
            }
        },
        {
            typeof(Mirabella), new List<string>()
            {
                "Hi, hi! I need a super-duper energy potion! I’m performing tonight, you see? Dancing all night. I need something that… really perks me up.",
                "Help! I’m performing tonight, a big dance show. I need a really strong energy potion to get me through it. I need something that’ll really wake me up"
            }
        },
        {
            typeof(Seraphina), new List<string>()
            {
                "Greetings, child of the stars. I see your aura… strong, but needing direction. I seek… a potion of insight. The veil between worlds has thinned, and I need to amplify my gift to foresee the future that is tangled like the threads of destiny",
                "I desperately need a potion of insight. The worlds are converging, and I must sharpen my ability to foresee the tangled fate before us!"
            }
        },
        {
            typeof(Theodore), new List<string>()
            {
                "Ah, yes, good day! Or rather, morning… or evening? Never mind! I need… oh, what did I want? Ah, yes! A potion! A potion for… him… what for? Ah, right! For memory! And for concentration!",
                "I came for… hmm, what was it? Oh, yes! A potion! A potion for… that fellow… what was it supposed to do? Oh, right! For memory! And… and for focusing!"
            }
        },
    };

    public static string GetRandomDialogue<T>()
    {
        DialogueAll.TryGetValue(typeof(T), out List<string> Dialogues);
        if (Dialogues == null)
            return null;
        
        return Dialogues[Random.Range(0, Dialogues.Count)];
    }


}
