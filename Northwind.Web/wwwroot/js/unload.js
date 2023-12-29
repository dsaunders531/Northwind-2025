// When a page unloads - disable all controls
//
// This stops any changes or resubmissions from being made
//

EnableUnload();

function EnableUnload() {
    window.addEventListener("beforeunload", (ev) => {
        DisableDocument();
                             
        document.querySelectorAll("button[type='submit'],input[type='submit'],button[type='reset'],input[type='reset']")
            .forEach((e) => {
                try {
                    // remove functionality
                    e.type = "button";
                } catch (ex) {
                    console.error(ex);
                }
            });   

        document.style.cursor = "progress";
    });

    window.addEventListener("DOMContentLoaded", (ev) => {
        EnableDocument();
    });
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