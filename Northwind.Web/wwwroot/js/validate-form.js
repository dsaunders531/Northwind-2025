
document.onreadystatechange = (ev) => {
    if (document.readyState == 'complete') {
        EnableValidator();
    }
};

function EnableValidator() {
    // form elements are input, select, textarea

    // ALSO: on submit - disable all controls (prevent repeat clicks)

    // see: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input

    var eles = ['button', 'fieldset', 'input', 'output', 'select', 'textarea'];

    // Listen to all the elments
    eles.forEach((ele) => {
        try {
            document.querySelectorAll(ele).forEach((q) => { Listen(q); });
        } catch (ex) {
            console.error(ex);
        }        
    });    
}


function Listen(inputEle) {    
    inputEle.addEventListener("input", (event) => {
        try {
            if (inputEle.validity.valid) {
                // set the validation label to empty and set the class to 'normal'
                // TODO

            } else {
                ShowError(inputEle);
            }
        } catch (e) {
            console.error(e);
        }
    });
}

function ShowError(inputEle) {
    if (inputEle.validity.valueMissing) {
        // If the field is empty,
        // display the following error message.

        // TODO set the error in the label
        //label.textContent = "You need to enter an email address.";
    } else if (inputEle.validity.typeMismatch) {
        // If the field doesn't contain an email address,
        // display the following error message.

        // TODO
        //emailError.textContent = "Entered value needs to be an email address.";
    } else if (inputEle.validity.tooShort) {
        // If the data is too short,
        // display the following error message.

        // TODO emailError.textContent = `Email should be at least ${email.minLength} characters; you entered ${email.value.length}.`;
    }

    // Set the styling appropriately
    emailError.className = "error active";
}