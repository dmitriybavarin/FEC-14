fec-mapping-tool-pick-tooltip =
    EYEDROPPER
    Takes any object or floor straight from the map and immediately starts placing the exact same thing.
    How to use: press the button, then left click the object you want.
    The object is taken with its rotation, so walls, doors, pipes and furniture are placed the same way as the sample.
    Clicking empty floor takes the floor type, and you will keep laying the same floor.
    After picking, the button turns itself off and every click places the picked thing.
    Rotate the object being placed with the middle mouse button. Stop placing with a right click.
    To cancel picking before the click, press the button again or right click.
fec-mapping-tool-pick-start = Click an object or floor to pick it
fec-mapping-tool-eraser-tooltip =
    ERASER
    Deletes objects from the map with a left click.
    While the button is highlighted, every click on an object permanently removes it from the server.
    Deletions go into the history, so a mistake can be fixed with the Undo button or Ctrl+Z.
    Be careful: the eraser deletes anything you click, including walls, machines and items lying on the floor.
    Press the button again to turn the eraser off.
fec-mapping-tool-select-tooltip =
    AREA SELECTION
    Copies a rectangular piece of the map: the floor and every object on it.
    How to use: press the button, left click the first corner of the area, then the opposite corner.
    The maximum size is 64 by 64 tiles. Players, any living creatures and the grids themselves are not copied.
    The copy is kept until the next copy and is placed with the Paste button.
    Without the button: hover each of the two corners and press Alt+C.
    To cancel before the second click, right click or press the button again.
fec-mapping-tool-select-start = Click the first corner of the area
fec-mapping-tool-paste-tooltip =
    AREA PASTE
    Pastes the area copied with the Area selection button.
    How to use: press the button and left click where the bottom left corner of the area should go.
    You can paste many times in a row while the button is highlighted.
    Pasting replaces the floor under the area and adds all copied objects on top of whatever is already there.
    Each paste is undone as a whole with one Undo or Ctrl+Z.
    Without the button: hover and press Alt+V.
    To turn paste off, right click or press the button again.
fec-mapping-tool-paste-start = Click where to paste the area
fec-mapping-tool-undo-tooltip =
    UNDO
    Undoes your last mapping action: placing an object, deleting, changing a floor tile or pasting an area.
    Each press undoes exactly one action. An area paste counts as one action.
    The history keeps the last 100 actions and is separate for each admin.
    The history is reset when the server restarts.
    Shortcut: Ctrl+Z.
fec-mapping-tool-redo-tooltip =
    REDO
    Brings back the action you have just undone.
    Works until you do something new: any new action clears the redo list.
    Shortcuts: Ctrl+Y or Ctrl+Shift+Z.
