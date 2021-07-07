# Better Chat

This is a mod that improves some annoyances with the in-game text chat.

- It prevents you from switching hotbar slots while typing in the chat
- It correctly unfocuses the chat box when you try to send an empty message (in vanilla, it's not active but the UI element still has focus, and pressing space selects it to start typing)
- Commands have no character limit (in vanilla, it crops every message *before* it even checks if it's a command)
- Command responses have no character limit (specifically, any message sent from clientID -1, which represents a system message, in vanilla incoming messages are always cropped before adding to the chat)
- You can use up/down arrow keys to restore chat messages you already sent (especially useful for commands, where you might make a typo and wanna fix it without retyping the whole command)
- Crucially, it does not reference or patch ``ChatCommand()``, so it shouldn't ever conflict with any mods that add/modify commands.

Thanks to [nociconist](https://thenounproject.com/search/?q=chat&i=1549183) for providing a free-to-use chat bubble icon that i legally have to credit. I'm not entirely sure if it counts, but just in case i also need to specify that i have modified the icon. Specifically, i've recolored it and added text, the actual icon is the same shape. On the off chance that the original artist is reading this, i would like to personally apologize for putting Comic Sans next to your icon.