// Validator for forms and input controls using asp.net razor server-side attributes.
// you could use this on any form following this pattern:
// <input data-val-required="this is required" required id="something" name="Something" />
// <span data-valmsg-for="Something" class="text-error"></span>
// If you are using razor server-side, you do not need to worry about adding the extra attributes manually.
// this does not have to be used for just forms.
// it can work with responsive pages too.
// in theory this could be added to every page.
// need to watch for content changes and reload.
EnableValidator();

function EnableValidator() {
    console.info('Validator Enabled!');
    // form elements are input, select, textarea
    // see: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input

    var eles = ['button', 'fieldset', 'input', 'output', 'select', 'textarea', 'form','option','optgroup','.btn'];

    // Listen to all the elments
    eles.forEach((ele) => {
        try {
            document.querySelectorAll(ele).forEach((q) => { Listen(q); });
        } catch (ex) {
            console.error(ex);
        }        
    });    

    // the form is going to be submitted. Disable everything
    window.addEventListener("beforeunload", (ev) => {
        try {
            if (validationObserver != null) {
                try {
                    validationObserver.disconnect();
                } catch (e) {
                    console.error(e);
                }                
            }
            
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
        } catch (e) {
            console.error('Disable document problem: ' + e);
        }    
    });    
}

var validationObserver = null;

document.onreadystatechange = function (ev) {
    if (document.readyState == 'complete') {
        // setup the observer
        console.info('Setup validation observer');
        validationObserver = new MutationObserver(observerCallback);
        validationObserver.observe(document.getElementsByTagName('body')[0], { attributes: true, childList: true, subtree: true });
    }   
};

// Using an observer for web apps:
const observerCallback = (mutationList, observer) => {
    if (mutationList.length) {
        console.info('Page has changed');
        // reset the validator.
        EnableValidator();
    }
};

function DisableDocument() {
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

function Listen(inputEle) {    
    try {
        inputEle.addEventListener("input", function (event) {
            HandleValid(event.currentTarget);
        });

        inputEle.addEventListener("change", function (event) {
            HandleValid(event.currentTarget);
        });

        inputEle.addEventListener("blur", function (event) {
            HandleValid(event.currentTarget);
        });

        inputEle.addEventListener("submit", function (event) {
            HandleValid(event.currentTarget);
            try {
                if (!event.currentTarget.validity.valid) {
                    event.stopPropagation();
                    event.preventDefault();
                    return false;
                }            
            } catch (ex) {
                console.error(ex);
            }            
        });

        inputEle.addEventListener("reset", function (event) {
            // clear all error messages
            document.querySelectorAll('span.text-danger').forEach((e) => {
                try {
                    e.innerText = '';
                } catch (ex) {
                    console.error(ex);
                }                
            });
            // enable submit
            document.querySelectorAll("button[type='submit'],input[type='submit']").forEach((e) => {
                try {
                    e.style.cursor = 'initial';
                    if (e.classList.contains('disabled')) {
                        e.classList.remove('disabled');
                    }
                    e.disabled = false;
                } catch (ex) {
                    console.error(ex);
                }
            });
        });

        inputEle.addEventListener("click", function (event) {
            if (!event.currentTarget.getAttribute('type') == 'reset') {
                HandleValid(event.currentTarget);
                try {
                    if (!event.currentTarget.validity.valid) {
                        event.stopPropagation();
                        event.preventDefault();
                        return false;
                    }   
                } catch (ex) {
                    console.error(ex);
                }                   
            }            
        });
    } catch (e) {
        console.error(e);
    }    
}

function HandleValid(ele) {    
    try {
        var targetName = ele.getAttribute('name');

        if (targetName != null) {
            console.info('Check validity ' + ele.id + ' name: ' + targetName);

            // span is <span data-valmsg-for="ele id">        
            var msgEle = document.querySelector("span[data-valmsg-for='" + targetName + "']");

            if (ele.validity.valid) {
                // set the validation label to empty
                document.querySelectorAll("button[type='submit'],input[type='submit']").forEach((e) => {
                    try {
                        e.style.cursor = 'initial';
                        if (e.classList.contains('disabled')) {
                            e.classList.remove('disabled');
                        }
                        e.disabled = false;
                    } catch (ex) {
                        console.error(ex);
                    }

                });
                msgEle.innerText = '';
                ele.setCustomValidity('');
                ele.reportValidity();
            } else {
                // show the error message;
                ShowError(ele, msgEle);

                document.querySelectorAll("button[type='submit'],input[type='submit']").forEach((e) => {
                    try {
                        e.style.cursor = 'not-allowed';
                        if (!e.classList.contains('disabled')) {
                            e.classList.add('disabled');
                        }
                        e.disabled = true;
                    } catch (ex) {
                        console.error(ex);
                    }
                });
            }
        }
        else {
            console.warn('Cannot validate ' + ele.id + ' ' + typeof ele);
        }
    } catch (e) {
        console.error(e);
    }
}

function ShowError(inputEle, msgEle) {
    console.warn(inputEle.id + ' is not valid!');

    // message is at input data-val-[type]="some message"  data-val-required="some message"
    // types are
    // button checkbox color date datetime datetime-local
    // email file hidden  image month number password radio
    // range reset search submit tel text time url week   
    // others are: required min max maxlength pattern step remote
    
    if (inputEle.validity.valueMissing) {
        // If the field is empty        
        msgEle.innerText = GetErrorMsg(inputEle, ['required', 'password','remote','search']);
    }
    else if (inputEle.validity.typeMismatch) {        
        msgEle.innerText = GetErrorMsg(inputEle, ['number','pattern','step','remote', 'color', 'date', 'datetime', 'datetime-local', 'file', 'image', 'month']);
    }
    else if (inputEle.validity.badInput) {
        msgEle.innerText = GetErrorMsg(inputEle, ['checkbox','color','date','datetime','datetime-local','email','file','image','month','number','password','radio','search','tel','text','time','url','week','pattern','step','remote']);
    }
    else if (inputEle.validity.customError) {
        msgEle.innerText = GetErrorMsg(inputEle, ['required', 'min', 'max', 'maxlength', 'pattern','step','remote','checkbox', 'color', 'date', 'datetime', 'datetime-local', 'email', 'file', 'image', 'month', 'number', 'password', 'radio', 'search', 'tel', 'text', 'time', 'url', 'week', 'pattern', 'step']);
    }
    else if (inputEle.validity.tooLong) {
        msgEle.innerText = GetErrorMsg(inputEle, ['max', 'maxlength', 'pattern', 'step', 'remote', 'color', 'date', 'datetime', 'datetime-local', 'email', 'file', 'image', 'month', 'number', 'password', 'radio', 'search', 'tel', 'text', 'time', 'url', 'week', 'pattern', 'step']);
    }
    else if (inputEle.validity.tooShort) {        
        msgEle.innerText = GetErrorMsg(inputEle, ['min', 'minlength', 'pattern', 'step', 'remote', 'color', 'date', 'datetime', 'datetime-local', 'email', 'file', 'image', 'month', 'number', 'password', 'radio', 'search', 'tel', 'text', 'time', 'url', 'week', 'pattern', 'step']);
    }       
    else if (inputEle.validity.patternMismatch) {
        msgEle.innerText = GetErrorMsg(inputEle, ['pattern', 'email', 'remote', 'search']);
    }
    else if (inputEle.validity.rangeOverflow) {
        msgEle.innerText = GetErrorMsg(inputEle, ['pattern', 'max', 'maxlength', 'step', 'number', 'date', 'datetime', 'datetime-local','time','month','time','week']);
    }
    else if (inputEle.validity.rangeUnderflow) {
        msgEle.innerText = GetErrorMsg(inputEle, ['pattern', 'min', 'minlength', 'step', 'number', 'date', 'datetime', 'datetime-local', 'time', 'month', 'time', 'week']);
    }    
    else if (inputEle.validity.stepMismatch) {
        msgEle.innerText = GetErrorMsg(inputEle, ['step', 'number', 'date', 'datetime', 'datetime-local', 'time', 'month', 'time', 'week']);
    } 
    else {
        // there must be an error somewhere!
        msgEle.innerText = GetErrorMsg(inputEle, ['button', 'checkbox', 'color', 'date', 'datetime', 'datetime-local', 'email', 'file', 'hidden', 'image', 'month', 'number', 'password', 'radio', 'range', 'reset', 'search', 'submit', 'tel', 'text', 'time', 'url', 'week', 'required', 'min', 'max', 'maxlength', 'pattern', 'step', 'remote']);
    }
}

function GetErrorMsg(inputEle, checkAttrs) {
    const defaultError = "Check your input";
    var result = (inputEle.validationMessage?.length ?? 0) == 0 ? defaultError : inputEle.validationMessage;

    if (result == defaultError) {
        // check the attributes - stop at the first one.
        var found;
        for (var i = 0; i < checkAttrs.length; i++) {
            found = ele.getAttribute('data-val-' + checkAttrs[i]);
            if (found.length) {
                result = found;
                break;
            }
        }
    }

    return result;
}