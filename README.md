# Spiderman
Start date: September 2018

### Overview
I started this project because I was unhappy that the new Spiderman game was only for PS4.

The character is just a ragdoll prototype. I did this so that each limb would react to physics and add the element of unpredictability. I also don’t know how to animate very well, so that was a major factor in my decision to use ragdolls.

Originally, I planned on using Bezier curves to simulate the swing. I realized this retracted from my intention to have physics applied to the character. Creating a stable, physics-based rope in Unity is largely difficult, so I opted for the [Obi Rope](https://assetstore.unity.com/packages/tools/physics/obi-rope-55579?aid=1100lJ2z&utm_source=aff) asset from the Unity Asset Store. 

For each rope, one end attaches to a hand on the character and the other end attaches to a structure or the ground. A raycast from the camera is performed to determine the position of the one end of the rope on a structure. The position of the character’s hand is already given by the character, so using these two positions give the desired effect. However, the rope often looks loose, so I stopped rendering the rope while still maintaining its physics and implemented a line renderer starting from and ending at the two positions to give the rope a more “taut” look.

![](https://media.giphy.com/media/kiulT4faq7acwc0P0D/giphy.gif)

### Problems

The ragdoll itself has a few problems. While the ragdoll is in the middle of a swing, it wants to dip lower than it can, so the joints stretch and cause the ragdoll to spring back into its “correct” position at a very high velocity. As a result, this causes the colliders of each limb to overlap, setting off a chain reaction of springing joints and overlapping colliders. Normally, the ragdoll will calm down a return to a stable state, but sometimes, the chain reaction reaches a point of no return. This same problem appears when the ragdoll crashes into another object at a very high velocity.

![](https://media.giphy.com/media/fxzFkr6wSQ3vKfv1bB/giphy.gif)

I’ve been working on trying to mitigate this issue as much as I can since this is more of a limitation of the Unity physics engine. For now, I enjoy swinging through the structures as a ragdoll Spiderman in his current state.
