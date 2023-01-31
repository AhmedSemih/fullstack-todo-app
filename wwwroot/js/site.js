const checkBoxes = document.querySelectorAll(".checkboxes");
const submitBtn = document.getElementById("submitBtn");

checkBoxes.forEach((checkbox) => {
    checkbox.addEventListener("click", () => {
        const button = document.getElementById(`submitBtn_${checkbox.id}`);
        setTimeout(() => {
            button.click();
        }, 2000);
    });
});