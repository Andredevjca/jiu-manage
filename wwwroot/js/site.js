'use strict';
document.getElementById('alternar-menu')?.addEventListener('click',()=>document.body.classList.toggle('menu-recolhido'));
document.querySelectorAll('[data-enviar]').forEach(campo=>campo.addEventListener('change',()=>campo.form.requestSubmit()));
document.querySelectorAll('[data-confirmar]').forEach(formulario=>formulario.addEventListener('submit',evento=>{if(!window.confirm(formulario.dataset.confirmar))evento.preventDefault();}));
document.querySelectorAll('[data-senha]').forEach(botao=>botao.addEventListener('click',()=>{const campo=document.getElementById(botao.dataset.senha);campo.type=campo.type==='password'?'text':'password';}));
document.querySelectorAll('[data-aba]').forEach(botao=>botao.addEventListener('click',()=>{document.querySelectorAll('[data-aba]').forEach(item=>{item.classList.remove('ativo');item.setAttribute('aria-selected','false');});botao.classList.add('ativo');botao.setAttribute('aria-selected','true');document.querySelectorAll('.aba-perfil').forEach(aba=>aba.hidden=aba.id!==botao.dataset.aba);}));
document.querySelectorAll('[data-imprimir]').forEach(botao=>botao.addEventListener('click',()=>window.print()));
document.querySelector('[data-marcar-todos]')?.addEventListener('change',evento=>document.querySelectorAll('input[name="presentes"]').forEach(campo=>campo.checked=evento.target.checked));
document.querySelectorAll('form').forEach(formulario=>formulario.addEventListener('submit',evento=>{if(evento.defaultPrevented)return;const botao=evento.submitter;if(botao){setTimeout(()=>{botao.disabled=true;botao.setAttribute('aria-busy','true');},0);setTimeout(()=>{botao.disabled=false;botao.removeAttribute('aria-busy');},15000);}}));
