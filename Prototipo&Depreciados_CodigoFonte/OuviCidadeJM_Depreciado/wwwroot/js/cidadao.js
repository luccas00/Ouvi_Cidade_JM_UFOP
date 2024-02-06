

function validarCPF(cpf) {

    let valido = true;

    cpf = cpf.replace(/\D/g, '');

    if (cpf.length !== 11) {
        valido = false;
    }

    if (/^(\d)\1+$/.test(cpf)) {
        valido = false;
    }

    let soma = 0;
    for (let i = 0; i < 9; i++) {
        soma += parseint(cpf.charat(i)) * (10 - i);
    }

    let resto = soma % 11;
    let digito1 = resto < 2 ? 0 : 11 - resto;

    soma = 0;
    for (let i = 0; i < 10; i++) {
        soma += parseint(cpf.charat(i)) * (11 - i);
    }

    resto = soma % 11;
    let digito2 = resto < 2 ? 0 : 11 - resto;

    if (parseint(cpf.charat(9)) !== digito1 || parseint(cpf.charat(10)) !== digito2) {
        valido = false;
    }

    if (valido === true) {
        Xrm.Navigation.openAlertDialog("CPF Valido !")
    } else {
        Xrm.Navigation.openAlertDialog("CPF Inválido!");
        document.getElementById("CPF").value = ""; // Limpa o campo
        document.getElementById("CPF").focus(); // Foca novamente no campo
    }

}
