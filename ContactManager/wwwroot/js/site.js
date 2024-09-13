document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.delete-btn').forEach(function (button) {
        button.addEventListener('click', function (event) {
            event.preventDefault();

            var row = this.closest('tr');
            var id = row.getAttribute('data-id');

            console.log('Deleting contact with ID:', id); 

            if (confirm('Are you sure you want to delete this contact?')) {
                fetch(`/Contacts/Delete/${id}`, {
                    method: 'DELETE'
                })
                    .then(response => {
                        console.log('Response status:', response.status);
                        if (response.ok) {
                            row.remove();
                        } else {
                            return response.text().then(text => {
                                alert('Failed to delete contact: ' + text);
                            });
                        }
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        alert('Failed to delete contact.');
                    });
            }
        });
    });
});
