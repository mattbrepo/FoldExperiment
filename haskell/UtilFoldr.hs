-- -----------------
--  foldr utilities
-- -----------------
module UtilFoldr where

-- distinct (e.g.: "ciaoc1" ==> "ciao1")
f_distinct [] = []
f_distinct xs = ((head xs):(foldr f [] [1..(length xs)-1])) -- 'c':"iao1"
  where f n vs = do
        -- caracter to check ('1')
        let l = head (drop n xs) 
        -- rest of string ("ciaoc")
        let ws = take n xs
        -- if it's present in rest of the string don't add it!
        if (elem l ws) then vs else (l:vs)

-- count
f_count a xs = foldr f 0 xs
  where f x v = if (x == a) then (v + 1) else v

-- replicate
f_replicate n v = foldr (\x vs -> v:vs) [] [1..n]

-- map
f_map f xs = foldr g [] xs
  where g x v = (f x):v

-- sort
f_sort xs = foldr f [] xs
  where f x [] = [x]
        f x vs = if x > (last vs) then vs ++ [x] else (f x (init vs)) ++ [last vs]

--- any
f_any v = foldr (\x y -> y || (x == v)) False

--- join
f_join sep = foldr f ""
  where f v x = if null x then v else v ++ sep ++ x