-- defines a factorial function
function fact(n)
    if (n == 0) then
        return 1
    else
        return n*fact(n - 1)
    end
end

a = function(num)
    print(num*2)
    return num*3
end

function getAuthor()
    return nil
end

print("Hello from the script #2")

for k, v in pairs({yo=5}) do
    print(k, v)
end

print(type(require))

print("Hello from the script", 1, 2, 3, true)

return true, 1, 2, 3 ,"hey"
