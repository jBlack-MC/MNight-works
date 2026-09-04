//grab a reference to the emoty <div id="menu-list"> from html above 
const menuListElement = document.getElementById("menu-list");

//"async function" is javaScript 's version of the async/await you already know from C# -
//it lets this function pause at "await " without freezing the whole page
async function loadMenu() {
    //fetch() sends an HTTP request.with no options, it defaults to GET
    //same idea as typing a URL into a browser, just done from code
    const response = await fetch("/api/MenuItems");

    //fetch() only throws an error for things like network failures
    //A 404 or 500 still counts as "success" to fetch() so we check response.ok ourselves
    if (!response.ok) {
        menuListElement.textContent = "Couldnt load the menu right now.";
        return;
    }

    //the response arrives as raw text first: .json() parses it into a real
    // javaScript array - the JS equivalent of deseralizing JSON in C#
    const items = await response.json();

    if (items.length === 0) {
        menuListElement.textContent = "No menu items found.";
        return;
    }
    //.map() runs this arrow function once per item building a new array of HTML strings.
    // the backtick string below is a "template literal" -${...} drops a real value into it 
    const itemsHtml = items.map(item => `
        <div class ="menu-item">
            <h3>${item.name}</h3>
            <p>${item.description ?? ""}</p>
            <p class="price">${item.price.toFixed(2)}</p>
        </div>
    `).join(""); //join() turns the array of strings into one big string

    //replace everything inside the menuListElement with this freshly build HTML
    menuListElement.innerHTML = itemsHtml;
}
//Defining loadMenu() above doesnt run it - this line actually call it 
loadMenu();

//Grab refernce to the form and its three input fields 
const addItemForm = document.getElementById("add-item-form");
const nameInput = document.getElementById("name-input");
const descriptionInput = document.getElementById("description-input");
const priceInput = document.getElementById("price-input");

//"submit" fires when the button is clicked (or enter is pressed in the field)
//the event +> {....} part is an "arrow function" - a short way to write
//a function,very common in javaScript.it behaves like a normal function
addItemForm.addEventListener("submit", async event => {
    //forms reload the whole page by default when submitted
    //preventDefault() stops that, since we're handling it oursleves with fetch().
    event.preventDefault();

    const newItem = {
        //tells the database "assign a new id yourself" -same pattern as in scalar"
        id: 0,
        name: nameInput.value,
        description: descriptionInput.value,
        //input values always arrive as text/strings even for number inputs
        //parseFloat() converts "45.00" into the actual number 45.00
        price: parseFloat(priceInput.value),
        isAvailable: true
    };

    const response = await fetch("/api/MenuItems", {
        //fetch() defaults to GET, So post must be stated explicitly
        method: "POST",
        headers: {
            //tells the server "the body below is JSON"
            "Content-Type": "application/json"
        },
        body: JSON.stringify(newItem) //converts the js object into an actual JSON text string
    });
    if (!response.ok) {
        alert("Couldnt add the item.Try again.");
        return;
    }

    //clears the input fields for the next entry
    addItemForm.reset();
    //re-fetches the list so the new item appears immediately 
    loadMenu();
});