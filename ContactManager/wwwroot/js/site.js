document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.delete-btn').forEach(function (button) {
        button.addEventListener('click', function (event) {
            event.preventDefault();

            var row = this.closest('tr');
            var id = row.getAttribute('data-id');

            if (confirm('Are you sure you want to delete this contact?')) {
                fetch(`/Contacts/Delete/${id}`, {
                    method: 'DELETE'
                })
                    .then(response => {
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
    
    document.querySelectorAll('.edit-btn').forEach(function (button) {
        button.addEventListener('click', function () {
            var row = this.closest('tr');
            toggleEditMode(row, true);
        });
    });

    document.querySelectorAll('.cancel-btn').forEach(function (button) {
        button.addEventListener('click', function () {
            var row = this.closest('tr');
            toggleEditMode(row, false);
        });
    });

    function toggleEditMode(row, isEditing) {
        row.querySelectorAll('.view-mode').forEach(function (span) {
            span.style.display = isEditing ? 'none' : '';
        });
        row.querySelectorAll('.edit-mode').forEach(function (input) {
            input.style.display = isEditing ? '' : 'none';
        });
        row.querySelector('.edit-btn').style.display = isEditing ? 'none' : '';
        row.querySelector('.save-btn').style.display = isEditing ? '' : 'none';
        row.querySelector('.cancel-btn').style.display = isEditing ? '' : 'none';
    }

    document.querySelectorAll('#filterName, #filterDateOfBirth, #filterMarried, #filterPhone, #filterSalary').forEach(input => {
        input.addEventListener('input', filterTable);
    });

    function filterTable() {
        const name = document.getElementById('filterName').value.toLowerCase();
        const dateOfBirth = document.getElementById('filterDateOfBirth').value;
        const married = document.getElementById('filterMarried').value;
        const phone = document.getElementById('filterPhone').value.toLowerCase();
        const salary = document.getElementById('filterSalary').value;

        document.querySelectorAll('#contactTableBody tr').forEach(row => {
            const rowName = row.querySelector('td:nth-child(1) .view-mode').textContent.toLowerCase();
            const rowDateOfBirth = row.querySelector('td:nth-child(2) .view-mode').textContent;
            const rowMarried = row.querySelector('td:nth-child(3) .view-mode').textContent.toLowerCase();
            const rowPhone = row.querySelector('td:nth-child(4) .view-mode').textContent.toLowerCase();
            const rowSalary = row.querySelector('td:nth-child(5) .view-mode').textContent;

            row.style.display = (name === '' || rowName.includes(name)) &&
                (dateOfBirth === '' || rowDateOfBirth === dateOfBirth) &&
                (married === '' || rowMarried === married) &&
                (phone === '' || rowPhone.includes(phone)) &&
                (salary === '' || rowSalary === salary)
                ? '' : 'none';
        });
    }
});
