using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Meta_xi.Application;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DBContext context;
    private readonly UserService userService;
    private readonly IGeneratedJwt generatedJwt;
    public readonly IRegisteredToReferLevel registeredToReferLevel;
    public UserController(DBContext dBContex, UserService service, IGeneratedJwt IgeneratedJwt , IRegisteredToReferLevel registeredToRefer)
    {
        context = dBContex;
        userService = service;
        generatedJwt = IgeneratedJwt;
        registeredToReferLevel = registeredToRefer;
    }
    //Endpoint para registrar un usuario
    [HttpPost("UserRegister")]
    public async Task<IActionResult> UserRegister(UserRegister userRegister)
    {
        string code ;
        bool isUnique;
        do{
           code = userRegister.GeneratedReferCode();
           isUnique = !await context.Users.AnyAsync(option => option.Code == code);
        }while(!isUnique);
        if (userRegister.Email != null)
        {
            var user = await context.Users.FirstOrDefaultAsync(option => option.Email == userRegister.Email);
            if (user != null)
            {
                return BadRequest("Ese correo ya ha sido usado ");
            }
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-]+$";
            Match match = Regex.Match(userRegister.Email, pattern);
            if (!match.Success)
            {
                return NotFound("Por favor entre un correo valido");
            }
            string pattern2 = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$";
            Match match1 = Regex.Match(userRegister.Password, pattern2);
            if (!match1.Success)
            {
                if (!Regex.IsMatch(userRegister.Password, "(?=.*[A-Z])"))
                {
                    return BadRequest("La contraseña debe tener al menos una mayúscula");
                }
                else if (!Regex.IsMatch(userRegister.Password, "(?=.*[a-z])"))
                {
                    return BadRequest("La contraseña debe tener al menos una minúscula");
                }
                else if (!Regex.IsMatch(userRegister.Password, "(?=.*\\d)"))
                {
                    return BadRequest("La contraseña debe tener al menos un número");
                }
                else if (!Regex.IsMatch(userRegister.Password, "(?=.*[^\\da-zA-Z])"))
                {
                    return BadRequest("La contraseña debe tener al menos un carácter especial");
                }
                else
                {
                    return BadRequest("La contraseña debe tener al menos 6 caracteres");
                }
            }
            User userToRegister = new User
            {
                Email = userRegister.Email,
                Password = userService.GeneratePassword(userRegister.Password),
                PhoneNumber = null,
                Token = generatedJwt.GeneratedToken(userRegister.Email, userRegister.Password),
                Code = code,
                referLevel1s = null,
                referLevel2s = null,
                referLevel3s = null
            };
            await context.Users.AddAsync(userToRegister);
            await context.SaveChangesAsync();
            
        }else{
            PhoneNumberValidator phoneNumberValidator = new PhoneNumberValidator();
        if (userRegister.PhoneNumber != null)
        {
            if (phoneNumberValidator.IsValidPhoneNumber(userRegister.PhoneNumber))
            {
                var PhoneIsregistered = await context.Users.FirstOrDefaultAsync(option => option.PhoneNumber == userRegister.PhoneNumber);
                if(PhoneIsregistered != null){
                    return NotFound("Ese número de telefono ya ha sido usado");
                }
                string pattern2 = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$";
                Match match1 = Regex.Match(userRegister.Password, pattern2);
                if (!match1.Success)
                {
                    if (!Regex.IsMatch(userRegister.Password, "(?=.*[A-Z])"))
                    {
                        return BadRequest("La contraseña debe tener al menos una mayúscula");
                    }
                    else if (!Regex.IsMatch(userRegister.Password, "(?=.*[a-z])"))
                    {
                        return BadRequest("La contraseña debe tener al menos una minúscula");
                    }
                    else if (!Regex.IsMatch(userRegister.Password, "(?=.*\\d)"))
                    {
                        return BadRequest("La contraseña debe tener al menos un número");
                    }
                    else if (!Regex.IsMatch(userRegister.Password, "(?=.*[^\\da-zA-Z])"))
                    {
                        return BadRequest("La contraseña debe tener al menos un carácter especial");
                    }
                    else
                    {
                        return BadRequest("La contraseña debe tener al menos 6 caracteres");
                    }
                }
            }
            User user = new User{
                Email = null,
                Password = userService.GeneratePassword(userRegister.Password),
                PhoneNumber = userRegister.PhoneNumber,
                Token = generatedJwt.GeneratedToken(userRegister.PhoneNumber, userRegister.Password),
                Code = code,
                referLevel1s = null,
                referLevel2s = null,
                referLevel3s = null
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();  
        }
        }
        if(userRegister.CodeReferrer != null){
            var father = await context.ReferLevel1s.FirstOrDefaultAsync(option => option.UniqueCodeReFerred == userRegister.CodeReferrer);
             Console.WriteLine(father == null);
            if(father != null){
                var grandfather = await context.ReferLevel1s.FirstOrDefaultAsync(option => option.UniqueCodeReFerred == father.UniqueCodeReferrer);
                if(grandfather != null){
                    await registeredToReferLevel.VerifyToReferLevel3(grandfather.UniqueCodeReferrer, code);
                }
                await registeredToReferLevel.VerifyToReferLevel2(father.UniqueCodeReferrer, code);
            }
            await registeredToReferLevel.VerifyToReferLevel1(userRegister.CodeReferrer, code);
        }
        return Ok("Usuario registrado correctamente");
    }
    //Endpoint para loguear un usuario 
    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserLogin userLogin){
        if(userLogin.Email != null){
            var user = await context.Users.FirstOrDefaultAsync(option => option.Email == userLogin.Email);
        if(user == null){
            return NotFound("Usuario no encontrado");
        }
        Console.WriteLine(user.Password);
        if(!userService.verifyPassword(userLogin.Password,user.Password)){
            return BadRequest("Contraseña incorrecta");
        }
        user.Token = generatedJwt.GeneratedToken(userLogin.Email, userLogin.Password);
        context.Entry(user).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(user.Token);
        }
        var username = await context.Users.FirstOrDefaultAsync(option => option.PhoneNumber == userLogin.PhoneNumber);
        if(username == null){
            return NotFound("Usuario no encontrado");
        }
        if(!userService.verifyPassword(userLogin.Password,username.Password)){
            return BadRequest("Contraseña incorrecta");
        }
        if(userLogin.PhoneNumber != null){
            username.Token = generatedJwt.GeneratedToken(userLogin.PhoneNumber, userLogin.Password);
        }
        context.Entry(username).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(username.Token);
    }
    [HttpPost("GetLink")]
    public async Task<IActionResult> GetLink(GetReferLink getReferLink){
        string code = string.Empty;
        string link = string.Empty;
        Console.WriteLine(getReferLink.Email.IsNullOrEmpty());
        if(getReferLink.Email.IsNullOrEmpty()){
            var user = await context.Users.FirstOrDefaultAsync(option => option.PhoneNumber == getReferLink.PhoneNumber);
            if(user == null){
                return NotFound("Usuario no encontrado");
            }
            if(user.Code != null){
                code = user.Code;
                link = $"https://meta-xi.vercel.app/login?code={code}";
            }
        }else {
            var user = await context.Users.FirstOrDefaultAsync(option => option.Email == getReferLink.Email);
            if(user == null){
                return NotFound("Usuario no encontrado");
            }
             if(user.Code != null){
                code = user.Code;
                link = $"https://meta-xi.vercel.app/login?code={code}";
            }
        }
        
        return Ok(link);
    }
}