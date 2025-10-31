async function getUsers() {
    const response = await fetch("/api/users", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok === true) {
        const users = await response.json();
        const rows = document.querySelector("tbody");
        rows.innerHTML = "";
        users.forEach(user => rows.append(row(user)));
    }
}

async function getUser(id) {
    const response = await fetch(`/api/users/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok === true) {
        const user = await response.json();
        document.getElementById("userId").value = user.id;
        document.getElementById("userName").value = user.name;
        document.getElementById("userAge").value = user.age;
    } else {
        const error = await response.json();
        console.log(error.message);
    }
}

async function createUser(name, age) {
    const response = await fetch("/api/users", {
        method: "POST",
        headers: { 
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            name: name,
            age: parseInt(age, 10)
        })
    });

    if (response.ok === true) {
        const user = await response.json();
        document.querySelector("tbody").append(row(user));
    } else {
        const error = await response.json();
        console.log(error.message);
    }
}

async function editUser(id, name, age) {
    const response = await fetch("/api/users", {
        method: "PUT",
        headers: { 
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            id: id,
            name: name,
            age: parseInt(age, 10)
        })
    });

    if (response.ok === true) {
        const user = await response.json();
        document.querySelector(`tr[data-rowid='${user.id}']`).replaceWith(row(user));
    } else {
        const error = await response.json();
        console.log(error.message);
    }
}

async function deleteUser(id) {
    const response = await fetch(`/api/users/${id}`, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });

    if (response.ok === true) {
        const user = await response.json();
        document.querySelector(`tr[data-rowid='${user.id}']`).remove();
    } else {
        const error = await response.json();
        console.log(error.message);
    }
}

function resetForm() {
    document.getElementById("userId").value = "";
    document.getElementById("userName").value = "";
    document.getElementById("userAge").value = "";
}

function row(user) {
    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", user.id);

    const nameTd = document.createElement("td");
    nameTd.append(user.name);
    tr.append(nameTd);

    const ageTd = document.createElement("td");
    ageTd.append(user.age);
    tr.append(ageTd);

    const linksTd = document.createElement("td");

    const editBtn = document.createElement("button"); 
    editBtn.append("Edit");
    editBtn.addEventListener("click", async () => await getUser(user.id));
    linksTd.append(editBtn);

    const removeBtn = document.createElement("button"); 
    removeBtn.append("Delete");
    removeBtn.addEventListener("click", async () => await deleteUser(user.id));
    linksTd.append(removeBtn);

    tr.appendChild(linksTd);

    return tr;
}

document.getElementById("resetBtn").addEventListener("click", () => resetForm());

document.getElementById("saveBtn").addEventListener("click", async () => {
    const id = document.getElementById("userId").value;
    const name = document.getElementById("userName").value;
    const age = document.getElementById("userAge").value;

    if (id === "")
        await createUser(name, age);
    else
        await editUser(id, name, age);

    resetForm();
});

getUsers();
