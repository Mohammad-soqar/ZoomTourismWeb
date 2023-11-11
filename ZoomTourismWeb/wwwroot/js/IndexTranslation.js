$(document).ready(function () {
   
    // Load the user's preferred language from localStorage, if available
    var userLanguage = localStorage.getItem('userLanguage');
    if (userLanguage) {
        // Set the language selector to the user's preferred language
        $('#languageSelector').val(userLanguage);
        // Load the user's preferred language
        loadLanguage(userLanguage);
    } else {
        // If no preference is set, load the default language (English)
        loadLanguage('en');
    }

    // Handle language selection change
    $('#languageSelector').change(function () {
        var selectedLanguage = $(this).val();
      
        // Store the user's language preference in localStorage
        localStorage.setItem('userLanguage', selectedLanguage);
        loadLanguage(selectedLanguage);
    });
});

function loadLanguage(language) {
  
    var currentPage = window.currentPage;
   
    $.getJSON('/Translation/languages.json', function (data) {
        var translations = data[language][currentPage];
        $('.translate').each(function () {
            var key = $(this).data('translate-key');
            $(this).text(translations[key]);
        });
    }).done(function () {
        console.log('JSON file loading completed.');
    }).fail(function (jqxhr, textStatus, error) {
        console.error('Error loading JSON file:', textStatus, error);
    });
}
