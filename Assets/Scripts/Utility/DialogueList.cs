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
                "Hello, young alchemist. I've heard that excellent potions are brewed here. I need a special potion for my plants. They've completely withered.",
                "Hey, alchemist! Your potions are famous. My plants are dying! Need something to revive them.",
                "Hello, alchemist. I understand you create excellent potions. I'm in need of one for my plants, as they are not thriving. Maybe you have a potion that solves this problem?"
            }
        },
        {
            typeof(Grail), new List<string>()
            {
             "I need something against the parasites or something that can cure me.",
             "I think I have parasites, so I need something to cure me, or something to protect against them if it's not too late"
            }
        },
        {
            typeof(Grimbold), new List<string>()
            {
                "I'm working in the mine. Give me something.",
                "Working the night shift in the mine. Got anything to help me see better?"
            }
        },
        {
            typeof(Isabella), new List<string>()
            {
                "Greetings, lord of the cauldron! I've heard that you can make potions as well as Merlin himself! I have a business proposal for you, you might need some ingredients, look what I have.",
                "Hail, potion-brewing extraordinaire! They say you're the modern-day Merlin! I've got a business proposition that's practically magical. Speaking of magic, you might need some ingredients… and lucky you, I brought some!"
            }
        },
        {
            typeof(Mirabella), new List<string>()
            {
                "Hi, hi! I need a super-duper energy potion! I'm performing tonight, you see? Dancing all night. I need something that… really perks me up.",
                "Help! I'm performing tonight, a big dance show. I need a really strong energy potion to get me through it. I need something that'll really wake me up"
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
        {
            typeof(Anya), new List<string>()
            {
                "I need a pain reliever, a potent one. I want something that'll put me down for a good nap, but obviously nothing lethal",
                "I need a potion for pain. Something that'll knock me on my ass for a few hours, but won't kill me."
            }
        },
        {
            typeof(Borin), new List<string>()
            {
                "I need something' to see better in the dark. The tunnels are getting' deeper, and my eyes ain't what they used to be."
            }
        },
        {
            typeof(Finch), new List<string>()
            {
               "This is a big day for me, I have a performance! But I'm so anxious, I need something…anything… that will calm my nerves",
                "I have a big performance today... and I... well, do you have anything that could... Calm my nerves?"
            }
        },
        {
            typeof(Isolde), new List<string>()
            {
                "You need fire protection, what do you have",
                "“I desperately need fire protection! What have you got?"
            }
        },

        {
            typeof(Foley), new List<string>()
            {
                "You need fire protection, what do you have",
                "I desperately need fire protection! What have you got?"
            }
        },
        {
            typeof(Backquit), new List<string>()
            {
                "You need fire protection, what do you have",
                "I desperately need fire protection! What have you got?"
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
