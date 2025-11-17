/**
 * Egyptian National ID to Birth Date Calculator
 * Automatically calculates and fills birth date from Egyptian National ID number
 *
 * Egyptian ID Format (14 digits): CYYMMDDSSSSSSS
 * C = Century (2 = 1900s, 3 = 2000s)
 * YY = Year
 * MM = Month
 * DD = Day
 * SSSSSSS = Unique identifier
 */

function initIdNumberBirthDateCalculator(idNumberFieldId, birthDateFieldId) {
    var idNumberField = document.getElementById(idNumberFieldId);
    var birthDateField = document.getElementById(birthDateFieldId);

    if (!idNumberField || !birthDateField) {
        console.warn('IdNumber or BirthDate field not found');
        return;
    }

    idNumberField.addEventListener('input', function(e) {
        var idNumber = e.target.value;

        // Only process if we have at least 7 digits
        if (idNumber.length >= 7) {
            var century, year, month, day;

            // First digit indicates century: 2 = 1900s, 3 = 2000s
            var centuryDigit = idNumber.charAt(0);

            if (centuryDigit === '2') {
                century = '19';
            } else if (centuryDigit === '3') {
                century = '20';
            } else {
                // Invalid century digit, don't auto-fill
                return;
            }

            // Extract year, month, day (positions 1-6)
            year = idNumber.substring(1, 3);
            month = idNumber.substring(3, 5);
            day = idNumber.substring(5, 7);

            // Validate month and day
            var monthNum = parseInt(month);
            var dayNum = parseInt(day);

            if (monthNum < 1 || monthNum > 12 || dayNum < 1 || dayNum > 31) {
                // Invalid date, don't auto-fill
                return;
            }

            // Construct the date string in YYYY-MM-DD format
            var birthDate = century + year + '-' + month + '-' + day;

            // Set the date input value
            birthDateField.value = birthDate;
        }
    });
}

/**
 * Initialize AJAX validation for Egyptian National ID
 * @param {string} idNumberFieldId - The ID of the ID number input field
 * @param {string} validationUrl - The URL to validate against
 * @param {number} servantId - The servant ID (0 for new, actual ID for edit)
 */
function initIdNumberValidation(idNumberFieldId, validationUrl, servantId) {
    servantId = servantId || 0;

    $(document).ready(function () {
        var idField = $('#' + idNumberFieldId);
        var errorSpan = $('span[data-valmsg-for="Servant.IdNumber"]');

        idField.on('blur', function () {
            var idNumber = $(this).val().trim();

            // Clear previous error
            errorSpan.text('').removeClass('field-validation-error').addClass('field-validation-valid');
            idField.removeClass('input-validation-error');

            if (idNumber === '') {
                return; // Empty is allowed (optional field)
            }

            // Show loading indicator
            errorSpan.text('جاري التحقق...').css('color', '#3498db');

            $.ajax({
                url: validationUrl,
                type: 'GET',
                data: { idNumber: idNumber, servantId: servantId },
                success: function (response) {
                    if (!response.isValid) {
                        errorSpan.text(response.message)
                            .removeClass('field-validation-valid')
                            .addClass('field-validation-error');
                        idField.addClass('input-validation-error');
                    } else {
                        errorSpan.text('✓ الرقم القومي صحيح').css('color', '#27ae60');
                    }
                },
                error: function () {
                    errorSpan.text('حدث خطأ أثناء التحقق من الرقم القومي')
                        .removeClass('field-validation-valid')
                        .addClass('field-validation-error');
                }
            });
        });
    });
}
