// When a page unloads by submit button being clicked - disable all controls
//
// This stops any changes or resubmissions from being made

EnableUnload();

function EnableUnload() {    
    document.querySelectorAll("button[type='submit'],input[type='submit']")
            .forEach((e) => {
                try {                    
                    e.onclick = () => DisableSubmitDisableDocument(e);                    
                } catch (ex) {
                    console.error(ex);
                }
            });   
}

function DisableSubmitDisableDocument(btn) {
    // Allow the button to be clicked once
    // wait 30ms then disable the page.
    // Otherwise the submit event will not fire
    try {        
        window.setTimeout(() => {
            console.info("disabling submit button");
            btn.type = "button"; // it will not submit anymore
            DisableDocument();            
        }, 30);  
    } catch (ex) {
        console.error(ex);
    }      
}

function DisableDocument() {
    console.warn("Disabling document");
    document.querySelectorAll(".btn, button, fieldset, input, output, textarea, select, option, optgroup, a, form")
        .forEach(function (ele) {
            try {
                if (!ele.classList.contains('disabled')) {
                    ele.classList.add("disabled");
                }
                ele.disabled = true;
                ele.style.cursor = "disabled";
            } catch (e) {
                console.error(e);
            }
        });
}

function EnableDocument() {
    console.warn("Enable document");
    document.querySelectorAll(".btn, button, fieldset, input, output, textarea, select, option, optgroup, a, form")
        .forEach(function (ele) {
            try {
                if (ele.classList.contains('disabled')) {
                    ele.classList.remove("disabled");
                }
                ele.disabled = false;                
            } catch (e) {
                console.error(e);
            }
        });
}